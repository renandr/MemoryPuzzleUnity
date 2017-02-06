using DG.Tweening;
using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// this object move to a target
    /// can use for 2D and 3D elements
    /// </summary>
    [AddComponentMenu("Animation/Move To Target")]
    public class MoveToTargetAnimationComponent : AAnimationComponent
    {
        [Tooltip("the end scale value for this object")]
        [SerializeField]
        private float endScale;

        [Tooltip("the target transform to follow")]
        [SerializeField] private Transform target;

        [Tooltip("the main camera for the scene")]
        [SerializeField]
        private Camera mainCamera;

        [Tooltip("the camera for the ui")]
        [SerializeField]
        public Camera uiCamera;

        private Vector3 tempPosition;

        private float tempDuration;

        /// <summary>
        /// Play the animation from begin on
        ///</summary>
        protected override void Play()
        {
            tempPosition = TargetPosition;
            tempDuration = AnimationDuration;

            StartAnimation();
        }

        /// <summary>
        /// Complete the animation with a short delay
        ///</summary>
        protected override void Skip()
        {
            Tween.timeScale = (Tween.Duration() - Tween.Elapsed() * 2); //calculate the rest speed and make it faster
        }

        /// <summary>
        /// Complete the animation directly
        ///</summary>
        protected override void Complete()
        {
            Tween.Complete(true);
        }

        /// <summary>
        /// Start the animation move to target
        /// includes move and scale
        ///</summary>
        private void StartAnimation()
        {
            //Stop the current tween
            Tween.Kill();
            //Start a new tween
            Tween = transform.DOMove(tempPosition, tempDuration).OnComplete(OnComplete); //move the object
            transform.DOScale(Vector3.one * endScale, tempDuration).SetEase(Ease.OutExpo); //scale the object
            Tween.OnUpdate(UpdateAnimation);
        }

        /// <summary>
        /// Update the target position when it is needed
        ///</summary>
        private void UpdateAnimation()
        {            
            Vector3 targetPosition = TargetPosition;

            tempDuration = tempDuration - Time.deltaTime; //calculate the elapsed duration

            if (targetPosition != tempPosition)
            {
                tempPosition = targetPosition;
                StartAnimation();
            }
        }

        /// <summary>
        /// Start the animation move to target
        ///</summary>
        private Vector3 TargetPosition
        {
            get
            {
                Vector3 position = target.position;

                //check if the target an UI element
                if (target as RectTransform)
                {
                    position = target.TransformPoint(target.transform.position); //get the ui world position
                    position = UICamera.WorldToViewportPoint(position); //convert the ui worldposition into ui camera viewport
                    position = MainCamera.ViewportToWorldPoint(position); //convert the ui viewport into maincamera worldpoint

                }

                return position;
            }
        }

        /// <summary>
        /// The camera for the scene
        ///</summary>
        public Camera MainCamera
        {
            get { return mainCamera; }
            set { mainCamera = value; }
        }

        /// <summary>
        //the camera for the ui
        ///</summary>
        public Camera UICamera
        {
            get { return uiCamera; }
            set { uiCamera = value; }
        }

        public Transform TargetTransform
        {
            get { return target;}
            set { target = value;}
        }

        public float TargetScale
        {
            get{ return endScale;}
            set{ endScale = value;}
        }
    }
}
