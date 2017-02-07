using GGS.CakeBox.Utils;
using UnityEngine;

namespace GGS.ScreenManagement
{
    public abstract class AObjectFollowingPanel : AObjectFollowingPanel<NullScreenProperties>
    {
        protected sealed override void OnProperties()
        {
            //Sealing
        }
    }

    /// <summary>
    /// A UI element, that stays at a given world position compared to it's respective camera.
    /// </summary>
    public abstract class AObjectFollowingPanel<TProperties> : APanel<TProperties> where TProperties : IScreenPropertiesVO
    {
        
        /// <summary>
        /// The offset that is applied to the target position before 
        /// calculating it into screen space.
        /// </summary>
        [Header("Object following")]
        [SerializeField]
        protected Vector3 worldOffset = Vector3.zero;


        protected Vector3 targetPosition;
        protected Transform targetTransform;

        protected RectTransform rootCanvasRectTransform;

        /// <summary>
        /// If true the camera used targetPosition instead of the 
        /// targetTransform.
        /// </summary>
        protected bool isFollowsObject;

        public Camera LookUpCamera { get; set; }

        protected override void PanelStart()
        {
            SetupRootTransform();
        }

        public void SetupRootTransform()
        {
            rootCanvasRectTransform = transform.root.GetComponent<RectTransform>();
        }


        protected sealed override void OnPanelOpen()
        {
            SetupRootTransform();
            OnWorldPanelOpen();
        }

        protected abstract void OnWorldPanelOpen();

        protected override void OnPanelClose()
        {
        }


        public void SetTarget(Transform target)
        {
            isFollowsObject = true;
            targetTransform = target;
        }

        public void SetTarget(Vector3 target)
        {
            isFollowsObject = false;
            targetPosition = target;
        }

        protected virtual void LateUpdate()
        {
            if (IsOpen && LookUpCamera != null && rootCanvasRectTransform != null)
            {
                Vector3 targetWorldPosition;

                if (isFollowsObject)
                {
                    // component not setup right skip updatecall
                    if (targetTransform == null)
                    {
                        return;
                    }
                    // use the current world position of traget transform
                    targetWorldPosition = targetTransform.position + worldOffset;
                }
                else
                {
                    // use the set world position
                    targetWorldPosition = targetPosition + worldOffset;
                }
                transform.localPosition = CameraHelper.WorldspacePositionInCanvasSpace(LookUpCamera, targetWorldPosition, rootCanvasRectTransform);
            }
        }
    }
}