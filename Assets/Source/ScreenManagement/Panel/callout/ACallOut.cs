using UnityEngine;
using System.Collections;
using GGS.CakeBox.Logging;
using GGS.CakeBox.Utils;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// The base View class for all call outs. 
    /// Every call out needs references to their top and bottom anchor.
    /// </summary>
    public abstract class ACallOut<TCallOutComponent> : APanel<CallOutScreenProperties>
       where TCallOutComponent : ACallOutComponent
    {
        
        [Header("Call Out anchors"), SerializeField]
        private GameObject anchorTop;

        [SerializeField]
        private GameObject anchorBottom;

        /// <summary>
        /// The calling call out component
        /// </summary>
        protected TCallOutComponent activatedCallOut { get; private set; }

        private bool isRegistered;


        protected override void OnPanelOpen()
        {
            isRegistered = false;

            //TODO Fix Callout  CoroutineHelper.StartCoroutine(RegisterDelayed());

            OnCallOutOpen();
        }

        /// <summary>
        /// Register to signals if needed
        /// </summary>
        protected abstract void OnCallOutOpen();

        protected sealed override void OnProperties()
        {
            if (CurrentProperties != null && CurrentProperties.CallOutComponent != null)
            {
                TCallOutComponent newCallOut = CurrentProperties.CallOutComponent as TCallOutComponent;
                if (newCallOut == activatedCallOut)
                {
                    // called twice from the same component... close
                    RequestClose();
                    return;
                }

                // if the panel just got updated the old calling
                // call out could still be disabled
                if (activatedCallOut != null)
                {
                    activatedCallOut.Interactable = true;
                }
                activatedCallOut = newCallOut;
            }
            else
            {
                activatedCallOut = null;
            }

            if (activatedCallOut == null)
            {
                GGLog.LogError("The given properties can't be used with the type " + typeof(TCallOutComponent).Name + ". The call is now broken.", ScreenSystem.LogType);
                RequestClose();
                return;
            }

            // set the current call out to not interactable
            // so we don't get open signal from them
            activatedCallOut.Interactable = false;

            // adjust view to anchor
            SetAnchor(activatedCallOut.GetComponent<RectTransform>());

            // call view
            OnCallOutProperties();
        }
        /// <summary>
        /// Calling call out has updated. Update the view.
        /// </summary>
        protected abstract void OnCallOutProperties();


        protected override void OnPanelClose()
        {
            /*TODO Fix Callout  
            if (isRegistered)
            {
                //TODO check to use input manager
                EasyTouch.On_TouchUp -= OnClick;
                EasyTouch.On_UIElementTouchUp -= OnClick;
                EasyTouch.On_TouchDown -= OnClick;
            }
            else
            {
                CoroutineHelper.StopCoroutine(RegisterDelayed());
            }
            //*/
            OnCallOutClose();
        }

        /*TODO Fix Callout  
        private void OnClick(Gesture gesture)
        {
            RequestClose();
        }
        //*/
        private IEnumerator RegisterDelayed()
        {
            yield return null;
            /*TODO Fix Callout  
            EasyTouch.On_TouchUp += OnClick;
            EasyTouch.On_UIElementTouchUp += OnClick;
            EasyTouch.On_TouchDown += OnClick;
            //*/
        isRegistered = true;
        }

        protected override void RequestClose()
        {
            base.RequestClose();
            if (activatedCallOut != null)
            {
                activatedCallOut.SetInteractableDelayed(true);
                activatedCallOut = null;
            }
        }

        /// <summary>
        /// Unregister to signals if needed
        /// </summary>
        protected abstract void OnCallOutClose();



        /// <summary>
        /// Position relative to given anchor. Is the anchor
        /// on the top half of the screen the call out is position below 
        /// else on top.
        /// </summary>
        /// <param name="anchor">The ui elment to position to</param>
        private void SetAnchor(RectTransform anchor)
        {
            // get world data for anchor
            RectTransform selfTransform = transform as RectTransform;
            if (selfTransform == null)
            {
                GGLog.LogError("View has no RectTransform. A CallOut should be on UI elment.", ScreenSystem.LogType);
                return;
            }
            
            Vector3[] corners = new Vector3[4];
            anchor.GetWorldCorners(corners);
            float halfWidth = (corners[3].x - corners[0].x) / 2f;
            float halfHeight = (corners[1].y - corners[0].y) / 2f;
            //TODO const for index... people are scared

            // get view port position 
            Vector3 anchorCenter = new Vector3(corners[0].x + halfWidth,
                corners[0].y + halfHeight, 0f);

            // TODO UI Camera static getter bla
            Vector2 anchorScreenPos = RectTransformUtility.WorldToScreenPoint(selfTransform.root.GetComponent<Canvas>().worldCamera, anchorCenter);
            bool attachOnTop = (anchorScreenPos.y / Screen.height) < 0.5f;

            // change anchors
            anchorTop.SetActive(!attachOnTop);
            anchorBottom.SetActive(attachOnTop);

            // change rect transform pivot
            selfTransform.pivot = attachOnTop ? new Vector2(0.5f, 0f) : new Vector2(0.5f, 1f);

            // set position to same object
            Vector3 position = new Vector3(corners[0].x + halfWidth,
                attachOnTop ? corners[1].y : corners[0].y,
                selfTransform.position.z);

            selfTransform.position = position;
        }
    }
}