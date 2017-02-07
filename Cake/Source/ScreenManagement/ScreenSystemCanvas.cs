using System.Collections.Generic;
using UnityEngine;

namespace GGS.ScreenManagement
{
    [RequireComponent(typeof(Canvas))]
    public class ScreenSystemCanvas : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private Transform tutorialLayerPlaceholder;

        [SerializeField]
        protected List<string> layersToHideOnDialog = new List<string>();

        [Header("AnimationConfiguration")]
        [SerializeField]
        public float StartAnimationDuration;

        [SerializeField]
        public float EndAnimationDuration;

        [SerializeField]
        public bool EnableAnimations;
#pragma warning restore 649

        public static float DefaultRenderDepth = 100f;

        private static ScreenSystemCanvas instance;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private RectTransform canvasRectTransform;

        private List<GameObject> gosToHideOnDialog;


        public void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject, true);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            canvas = gameObject.GetComponent<Canvas>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasRectTransform = canvas.GetComponent<RectTransform>();
            DontDestroyOnLoad(UICamera.gameObject);
            DefaultRenderDepth = UICamera.depth;

            FindLayersToHideOnDialog();
        }

        protected void Start()
        {
            DialogLayer.FullscreenDialogOnScreen+=OnHideRender;
        }

        protected void OnDestroy()
        {
            DialogLayer.FullscreenDialogOnScreen-=OnHideRender;
        }

        private void OnHideRender(bool hide)
        {
            for (int i = 0; i < gosToHideOnDialog.Count; i++)
            {
                gosToHideOnDialog[i].SetActive(!hide);
            }
        }

        public Camera UICamera
        {
            get
            {
                return canvas.worldCamera;
            }
        }

        public Canvas UICanvas
        {
            get
            {
                return canvas;
            }
        }

        public static ScreenSystemCanvas Instance
        {
            get
            {
                return instance;
            }
        }

        public RectTransform CanvasRect
        {
            get
            {
                return canvasRectTransform;
            }
        }

        public bool TutorialLayerRegistered { get; private set; }

        public void RegisterTutorialLayer(Transform tutorialLayer)
        {
            tutorialLayer.SetParent(tutorialLayerPlaceholder, false);
            TutorialLayerRegistered = true;
        }

       

        private void FindLayersToHideOnDialog()
        {
            gosToHideOnDialog = new List<GameObject>();

            PanelLayer[] allChildLayers = GetComponentsInChildren<PanelLayer>();

            for (int i = 0; i < allChildLayers.Length; i++)
            {
                // hashing with a hashset is slower on this little count
                if (layersToHideOnDialog.Contains(allChildLayers[i].Id))
                {
                    gosToHideOnDialog.Add(allChildLayers[i].gameObject);
                }
            }
        }
        public void Show()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public void Hide()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }

    }
}