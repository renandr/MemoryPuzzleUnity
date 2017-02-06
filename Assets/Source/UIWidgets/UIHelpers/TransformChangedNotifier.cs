using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.goodgamestudios.warlands.uiWidgets
{
    /// <summary>
    /// This class calls an event every time its transform changes. This is currently a helper class to UIAnchorToObject
    /// but can be used for any other situations where you might want to avoid polling every frame at another object.
    /// Currently, if there's no one registered to OnTransformChanged, it automatically destroys itself.
    /// </summary>
    [ExecuteInEditMode]
    public class TransformChangedNotifier : MonoBehaviour
    {
        public System.Action OnTransformChanged;

        private Transform thisTransform;

        private void Awake()
        {
            thisTransform = transform;
        }

        /// <summary>
        /// Verifies every frame if the transform.hasChanged. If so, executes TransformChanged()
        /// </summary>
        private void Update()
        {
            if (thisTransform.hasChanged)
            {
                TransformChanged();
            }
        }

        /// <summary>
        /// This is ran every time this object's transform.hasChanged is set to true
        /// </summary>
        private void TransformChanged()
        {
            if (OnTransformChanged != null)
            {
                OnTransformChanged();
            }
            else
            {
                CleanUp();
            }
        }

        /// <summary>
        /// Automatically deletes the component if there's no one registered to its OnTransformChanged Action
        /// </summary>
        public void CleanUp()
        {
            if (OnTransformChanged == null)
            {
                DestroyImmediate(this);
            }
        }
    }
}