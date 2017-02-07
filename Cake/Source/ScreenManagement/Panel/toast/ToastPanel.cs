using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// This the view class that represents a Toast message.
    /// It listens for state changes of the Animator to animate the transitions.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Button))]
    public class ToastPanel : APanel, IAnimatorStateNotifyable<CustomAnimationState>
    {
        /// <summary>
        /// The duration a Toast message should be fully visible.
        /// </summary>
#pragma warning disable 649
        [SerializeField]
        private float ShowDuration = 3f;

        [Header("Setup")]
        [SerializeField]
        private Text contentText;

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Image image;

        [Header("Config")]
        [SerializeField]
        private ToastSpriteDictionary toastSpriteDictionary;

#pragma warning restore 649

        private readonly Queue<ToastVO> queue = new Queue<ToastVO>();

        private Animator animator;
        private int isVisibleBool;
        private Button button;
        private Action toastClickCallBack;

        private float countDown;
        private CustomAnimationState state;

        protected override void OnPanelOpen()
        {
            ScreenSystem.ToastRequested+=OnShowToast;
        }

        protected override void OnPanelClose()
        {
            ScreenSystem.ToastRequested+=OnShowToast;
        }

        private void OnShowToast(ToastVO toastVO)
        {
            if (!ScreenSystem.IsOpeningScreensEnabled(CommonLayerIds.Toasts))
            {
                return;
            }

            DisplayToast(toastVO);
        }

        private void DisplayToast(ToastVO vo)
        {
            AddData(vo);
        }

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            isVisibleBool = Animator.StringToHash("IsVisible");
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonPress);
        }


        public override void Update()
        {
            // no base call, we don't fade
            if (state == CustomAnimationState.Visible && countDown > 0f)
            {
                countDown -= Time.deltaTime;
                if (countDown < 0f)
                {
                    SetAnimatorVisibility(false);
                }
            }
        }

        protected override void PanelStart()
        {
            // start hidden
            Intractable = false;
            // gameObject.SetActive(false);
            state = CustomAnimationState.Invisible;
        }

        public void AddData(ToastVO vo)
        {
            queue.Enqueue(vo);

            if (queue.Count == 1)
            {
                ApplyVo(vo);
            }
        }

        private void ApplyVo(ToastVO vo)
        {
            Sprite icon;
            // get icon
            if (vo.Icon != null)
            {
                icon = vo.Icon;
            }
            else
            {
                toastSpriteDictionary.TryGetValue(vo.IconType, out icon);
            }

            if (icon == null)
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = icon;
                image.enabled = true;
            }

            titleText.text = vo.Title;
            contentText.text = vo.Content;
            toastClickCallBack = vo.ClickCallback;
            Intractable = toastClickCallBack != null;

            fadeGroup.blocksRaycasts = true;
            SetAnimatorVisibility(true);
            countDown = ShowDuration;
        }


        /// <summary>
        /// Impelementation of IAnimatorStateNotifyable. This gets called by the Animator when the
        /// "Invisible" or "Visible" states will be entered.
        /// </summary>
        /// <param name="layerIndex">The animation layer.</param>
        /// <param name="newState">The new view state.</param>
        public void StateChanged(int layerIndex, CustomAnimationState newState)
        {
            if(state == newState)
            {
                return;
            }

            state = newState;
            switch (state)
            {
                case CustomAnimationState.Invisible:
                    EnqueueNext();
                break;

                case CustomAnimationState.Visible:
                    // nothing currently
                break;
            }
        }

        /// <summary>
        /// Forwards the the visibility to the animator.
        /// Use this methode to change the view state.
        /// </summary>
        private void SetAnimatorVisibility(bool value)
        {
            if (animator.GetBool(isVisibleBool) != value)
            {
                animator.SetBool(isVisibleBool, value);
            }
        }

        private void EnqueueNext()
        {
            queue.Dequeue ();

            if(queue.Count > 0)
            {
                ApplyVo(queue.Peek());
            }
            else
            {
                fadeGroup.blocksRaycasts = false;
            }
        }


        /// <summary>
        /// Set the button on the gameobject interactable or not.
        /// </summary>
        public bool Intractable
        {
            get
            {
                return button.interactable;
            }

            set
            {
                button.interactable = value;
            }
        }

        /// <summary>
        /// Call back for unity button component. Forwards this to the ToastClicked event.
        /// </summary>
        public void OnButtonPress()
        {
            if (toastClickCallBack != null)
            {
                toastClickCallBack();
            }
        }


    }
}