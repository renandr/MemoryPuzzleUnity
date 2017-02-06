using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{


    public abstract class APanel : APanel<NullScreenProperties>
    {
        protected sealed override void OnProperties()
        {
            //Sealing
        }
    }


    public abstract class APanel<TProperties> : AScreen <TProperties, IPanelModelReader>
        where TProperties : IScreenPropertiesVO
    {
        protected float fadeStep = 4f;
        protected float timeToFadeIn = 0.8f;
        protected float counterToFadeIn;

        [SerializeField]
        protected CanvasGroup fadeGroup;

        [SerializeField]
        private bool deactivateOnDialogOpen;


        private bool originalInteractable;
        private bool originalBlocksRaycasts;

        public bool ForcedFadeOut { get; set; }

        public GenericEvent<float> UpdatedEvent { get; set; }

        public FadeState CurrentFadeState { get; protected set; }



        protected override void Awake()
        {
            base.Awake();
            if (fadeGroup != null)
            {
                originalInteractable = fadeGroup.interactable;
                originalBlocksRaycasts = fadeGroup.blocksRaycasts;
            }
        }

        protected sealed override void ScreenStart()
        {
            base.ScreenStart();
            if (fadeGroup != null)
            {
                ScreenSystem.FadeOutPanelsRequested+=ForceFadeOutBehavior;
            }
            PanelStart();
        }

        protected virtual void PanelStart()
        {
        }


        override protected void OnScreenDestroy()
        {
            UpdatedEvent = null;
            OnClose();
            if (fadeGroup != null)
            {
                ScreenSystem.FadeOutPanelsRequested -= ForceFadeOutBehavior;
            }
            OnPanelDestroy();
        }

        protected virtual void OnPanelDestroy()
        {
        }

        protected sealed override void OnOpen()
        {
            if (deactivateOnDialogOpen)
            {
                DialogLayer.FullscreenDialogOnScreen+=OnFullScreenDialog;
            }
            OnPanelOpen();
        }
        protected abstract void OnPanelOpen();

        protected override sealed void OnClose()
        {
            if (deactivateOnDialogOpen)
            {
                DialogLayer.FullscreenDialogOnScreen-=OnFullScreenDialog;
            }
            OnPanelClose();
        }

        protected abstract void OnPanelClose();

        private void OnFullScreenDialog(bool obj)
        {
            gameObject.SetActive(!obj);
        }

        protected virtual void RequestClose()
        {
            ScreenSystem.ClosePanel(ScreenId);
        }

        private void ForceFadeOutBehavior(bool forceFadeOut)
        {
            ForcedFadeOut = forceFadeOut;
            FadeOut();
        }


        public virtual void Update()
        {
            CheckForFading();

            if (fadeGroup != null)
            {
                DoFading(Time.deltaTime);
            }
        }

        protected virtual void CheckForFading()
        {
            float deltaTime = Time.deltaTime;
            counterToFadeIn -= deltaTime;
            if (counterToFadeIn < 0 && !ForcedFadeOut)
            {
                FadeIn();
            }
        }

        public void FadeIn()
        {
            switch (CurrentFadeState)
            {
                case FadeState.None:
                case FadeState.FadeOut:
                    CurrentFadeState = FadeState.FadeIn;
                    break;
            }
        }

        public void FadeOut()
        {
            counterToFadeIn = timeToFadeIn;
            switch (CurrentFadeState)
            {
                case FadeState.None:
                case FadeState.FadeIn:
                    CurrentFadeState = FadeState.FadeOut;
                    break;
            }
        }

        private void DoFading(float deltaTime)
        {
            switch (CurrentFadeState)
            {
                case FadeState.FadeIn:
                    DoFadeIn(deltaTime);
                    break;
                case FadeState.FadeOut:
                    DoFadeOut(deltaTime);
                    break;
            }
        }

        private void DoFadeIn(float deltaTime)
        {
            if (fadeGroup.alpha >= 1.0f)
            {
                CurrentFadeState = FadeState.None;
                fadeGroup.interactable = originalInteractable;
                fadeGroup.blocksRaycasts = originalBlocksRaycasts;
            }
            else
            {
                fadeGroup.alpha += fadeStep * deltaTime;
            }
        }

        private void DoFadeOut(float deltaTime)
        {
            if (fadeGroup.alpha <= 0.0f)
            {
                CurrentFadeState = FadeState.None;
                fadeGroup.interactable = false;
                fadeGroup.blocksRaycasts = false;
            }
            else
            {
                fadeGroup.alpha -= fadeStep * deltaTime;
            }
        }
    }
}