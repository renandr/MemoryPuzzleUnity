using UnityEngine;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// This component aligns an UI Element to an object other than its parent.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof (RectTransform))]
    public class UIAnchorToObject : MonoBehaviour
    {
        /// <summary>
        /// The possible attachment points for this
        /// object to align to
        /// </summary>
        private enum AnchoringType
        {
            Top,
            TopRight,
            Right,
            BottomRight,
            Bottom,
            BottomLeft,
            Left,
            TopLeft
        }

#pragma warning disable 649
        [SerializeField]
        private RectTransform alignTo;

        [SerializeField]
        private Vector2 offset;

        [SerializeField]
        private AnchoringType anchoring = AnchoringType.Right;
#pragma warning restore 649

        private TransformChangedNotifier notifier;
        private TransformChangedNotifier previousNotifier;
        private RectTransform rectTransform;
        private Vector3[] alignRectCorners;

        private void Awake()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            FetchNotifier();
        }

        /// <summary>
        /// This is called by Unity whenever this MonoBehaviour's parameters change.
        /// In this case, we refresh the object that this should be anchored to and
        /// reposition it.
        /// </summary>
        private void OnValidate()
        {
            FetchNotifier();
            Reposition();
        }

        /// <summary>
        /// Fetches the TransformChangedNotifier for the given alignTo object. In case one doesn't exist,
        /// it creates one. In case the alignTo changes, it attempts to clean up the previously registered listener.
        /// </summary>
        private void FetchNotifier()
        {
            if (alignTo == null)
            {
                CleanupSelf();
                return;
            }
            TransformChangedNotifier newNotifier = alignTo.gameObject.GetComponent<TransformChangedNotifier>();

            if (newNotifier != null)
            {
                previousNotifier = notifier;
                notifier = newNotifier;
                newNotifier.OnTransformChanged += Reposition;
            }
            else
            {
                notifier = alignTo.gameObject.AddComponent<TransformChangedNotifier>();
                notifier.OnTransformChanged += Reposition;
            }

            if (previousNotifier != notifier && previousNotifier != null)
            {
                previousNotifier.OnTransformChanged -= Reposition;
            }
        }

        private void OnDestroy()
        {
            CleanupSelf();
        }

        /// <summary>
        /// Removes the listeners from any events that this object might have associated to
        /// </summary>
        private void CleanupSelf()
        {
            if (notifier != null)
            {
                notifier.OnTransformChanged -= Reposition;
            }
            if (previousNotifier != null)
            {
                previousNotifier.OnTransformChanged -= Reposition;
            }
        }

        /// <summary>
        /// Repositions this object based on its configurations.
        /// </summary>
        private void Reposition()
        {
            if (alignTo == null)
            {
                return;
            }
            if (alignRectCorners == null)
            {
                alignRectCorners = new Vector3[4];
            }

            alignTo.GetWorldCorners(alignRectCorners);
            Vector2 newPosition = Vector2.zero;

            float halfWidth = (alignRectCorners[3].x - alignRectCorners[0].x) / 2f;
            float halfHeight = (alignRectCorners[1].y - alignRectCorners[0].y) / 2f;

            switch (anchoring)
            {
                case AnchoringType.Top:
                    newPosition = new Vector2(alignRectCorners[1].x + halfWidth, alignRectCorners[1].y);
                    break;
                case AnchoringType.TopRight:
                    newPosition = new Vector2(alignRectCorners[2].x, alignRectCorners[2].y);
                    break;
                case AnchoringType.Right:
                    newPosition = new Vector2(alignRectCorners[3].x, alignRectCorners[3].y + halfHeight);
                    break;
                case AnchoringType.BottomRight:
                    newPosition = new Vector2(alignRectCorners[3].x, alignRectCorners[3].y);
                    break;
                case AnchoringType.Bottom:
                    newPosition = new Vector2(alignRectCorners[0].x + halfWidth, alignRectCorners[0].y);
                    break;
                case AnchoringType.BottomLeft:
                    newPosition = new Vector2(alignRectCorners[0].x, alignRectCorners[0].y);
                    break;
                case AnchoringType.Left:
                    newPosition = new Vector2(alignRectCorners[0].x, alignRectCorners[0].y + halfHeight);
                    break;
                case AnchoringType.TopLeft:
                    newPosition = new Vector2(alignRectCorners[1].x, alignRectCorners[1].y);
                    break;
            }
            if (rectTransform == null)
            {
                rectTransform = gameObject.GetComponent<RectTransform>();
            }

            rectTransform.position = newPosition + offset;
        }
    }
}