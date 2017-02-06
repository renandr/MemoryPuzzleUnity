using GGS.CakeBox.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace GGS.UIWidgets
{

    //[ExecuteInEditMode]
    public class UICutoutPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform container;

        [SerializeField] private RectTransform topPanel;

        [SerializeField] private RectTransform midContainer;

        [SerializeField] private RectTransform bottomPanel;

        [SerializeField] private RectTransform leftPanel;

        [SerializeField] private RectTransform centerPanel;

        [SerializeField] private RectTransform rightPanel;

        [SerializeField] private Vector2 size;

        [SerializeField] private Vector2 position;

        [SerializeField] private bool visible;

        [SerializeField] private GameObject gameObjectToHighlight;

        [SerializeField] private bool clamped;

        [SerializeField] private bool disableAfterAnimation;

        private Camera uiCamera;
        private Vector3[] corners;
        private Image centerImage;
        private Animation anim;

        private bool updateEveryFrame
        {
            get { return gameObjectToHighlight != null; }
        }

        public Camera UICamera
        {
            get
            {
                if (uiCamera == null)
                {
                    uiCamera = container.root.GetComponent<Canvas>().worldCamera;
                }
                return uiCamera;
            }
        }

        public Vector2 SizeFactor { get; set; }

        private CanvasGroup[] canvasGroups;

        private void Awake()
        {
            canvasGroups = gameObject.GetComponentsInChildren<CanvasGroup>();
            anim = GetComponent<Animation>();
        }

        private void OnEnable()
        {
            if (anim!=null && disableAfterAnimation)
            {
                Invoke("DisableGO", anim.clip.length);
            }
        }

        private void DisableGO()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            DoValidation();
        }

        private void Update()
        {
            if (updateEveryFrame)
            {
                DoValidation();
            }
        }

        /* Uncomment this for live recalculation. Enabling this causes
 * the prefab to be marked as dirty every time, so it will
 * keep bothering your commits
 * 
#if UNITY_EDITOR
    private void OnValidate()
    {
        DoValidation();
    }
#endif
    */

        private void DoValidation()
        {
            if (gameObjectToHighlight != null)
            {
                CalculateHighlight(gameObjectToHighlight);
            }

            RecalculateAll();
            SetVisibility(visible);
        }

        private void CalculateHighlight(GameObject go)
        {
            // If it's a UI element, it'll have a RectTransform
            // otherwise, it's a worldspace model, which will have regular Transform
            RectTransform trans = gameObjectToHighlight.GetComponent<RectTransform>();
            if (trans != null)
            {
                size.x = trans.rect.width;
                size.y = trans.rect.height;

                position = GetRelativePosition(trans);

                if (SizeFactor != Vector2.zero)
                {
                    Vector2 factoredSize = new Vector2(size.x*SizeFactor.x, size.y*SizeFactor.y);
                    position += (size - factoredSize)/2;
                    size = factoredSize;
                }
            }
            else
            {
                Transform worldTrans = gameObjectToHighlight.GetComponent<Transform>();
                position = GetRelativeWorldPosition(worldTrans);
            }
        }

        public void SetRound(bool isRound)
        {
            if (centerImage == null)
            {
                centerImage = centerPanel.GetComponent<Image>();
            }
            centerImage.enabled = isRound;
        }

        public void SetVisibility(bool overlayVisible)
        {
            if (canvasGroups == null)
            {
                return;
            }
            for (int i = 0; i < canvasGroups.Length; i++)
            {
                canvasGroups[i].alpha = overlayVisible ? 1f : 0f;
            }
            visible = overlayVisible;
        }

        public void SetHighlightSize(Vector2 rectSize)
        {
            size = rectSize;
        }

        public void SetHighlightPosition(Vector2 pos)
        {
            position = pos;
        }

        public void SetObjectToHighlight(GameObject go)
        {
            gameObjectToHighlight = go;
            if (go != null)
            {
                CalculateHighlight(go);
                RecalculateAll();
            }
        }

        public void SetActive(bool active)
        {
            container.gameObject.SetActive(active);
        }

        public void RecalculateAll()
        {
            if (clamped)
            {
                ClampPosition();
            }

            centerPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            centerPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            centerPanel.anchoredPosition = new Vector2(position.x, centerPanel.anchoredPosition.y);

            midContainer.sizeDelta = new Vector2(midContainer.sizeDelta.x, size.y);
            midContainer.anchoredPosition = new Vector2(midContainer.anchoredPosition.x, position.y);
            if (clamped)
            {
                leftPanel.sizeDelta = new Vector2(Mathf.Abs(position.x), leftPanel.sizeDelta.y);
                bottomPanel.sizeDelta = new Vector2(bottomPanel.sizeDelta.x, Mathf.Abs(position.y));
            }
            else
            {
                leftPanel.sizeDelta = new Vector2(position.x, leftPanel.sizeDelta.y);
                bottomPanel.sizeDelta = new Vector2(bottomPanel.sizeDelta.x, position.y);
            }
            rightPanel.sizeDelta = new Vector2(container.rect.width - (leftPanel.rect.width + size.x),
                rightPanel.sizeDelta.y);
            topPanel.sizeDelta = new Vector2(topPanel.sizeDelta.x,
                container.rect.height - (bottomPanel.rect.height + size.y));
        }

        private void ClampPosition()
        {
            position.x = (int) Mathf.Clamp(position.x, 0f, container.rect.width - size.x);
            position.y = (int) Mathf.Clamp(position.y, 0f, container.rect.height - size.y);
        }

        public Vector2 GetRelativePosition(RectTransform objTransform)
        {
            if (corners == null)
            {
                corners = new Vector3[4];
            }
            objTransform.GetWorldCorners(corners);
            return CameraHelper.UIElementPositionInViewportSpace(UICamera, corners[0], container);
        }

        private Vector2 GetRelativeWorldPosition(Transform worldTrans)
        {
            Bounds bounds = CalculateBounds(worldTrans);
            Rect boundsRect = ProcessBounds(bounds);
            float max = Mathf.Max(boundsRect.width, boundsRect.height);
            size.x = max;
            size.y = max;

            return CameraHelper.UIElementPositionInViewportSpace(Camera.main, bounds.min, container);
        }

        private Bounds CalculateBounds(Transform worldTrans)
        {
            // If we ever want to join several bounds, this is the way to do it
            // however, usually leads to some visual bugs.
            /*Renderer[] objRenderers = worldTrans.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds();
        for (int i = 0; i < objRenderers.Length; i++)
        {
            bounds.Encapsulate(objRenderers[i].bounds);
        }
        
        return bounds;*/
            return worldTrans.GetComponentInChildren<Renderer>().bounds;
        }

        private Rect ProcessBounds(Bounds bounds)
        {
            return Camera.main.CalculateViewportSpaceBounds(bounds, container);
        }

    }
}
