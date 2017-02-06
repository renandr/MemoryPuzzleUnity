using System.Diagnostics;
using UnityEngine;

namespace GGS.CakeBox.Utils
{
    public static class GameObjectExtensionMethods
    {
        /// <summary>
        /// Destroys a game object
        /// </summary>
        /// <param name="go">The game object to destroy</param>
        public static void Destroy(this GameObject go)
        {
            Object.Destroy(go);
        }

        /// <summary>
        /// Utility function to set the name of a game object.
        /// This is only compiled into the editor code and does not influence the names in mobile builds.
        /// </summary>
        /// <param name="go">The game object to be renamed</param>
        /// <param name="name">Name for the game object</param>
        [Conditional("UNITY_EDITOR")]
        public static void SetNameInEditorOnly(this GameObject go, string name)
        {
            go.name = name;
        }

        /// <summary>
        /// Destroys all children of a game object
        /// Shortcut for <see cref="TransformExtensionMethods.DestroyAllChildren"/>
        /// </summary>
        /// <param name="parent">The parent game object whose children will be destroyed</param>
        public static void DestroyAllChildren(this GameObject parent)
        {
            parent.transform.DestroyAllChildren();
        }

        /// <summary>
        /// Destroys all children of a game object immediately
        /// Should be used in the editor only, see: http://docs.unity3d.com/ScriptReference/Object.DestroyImmediate.html
        /// Shortcut for <see cref="TransformExtensionMethods.DestroyAllChildrenImmediate"/>
        /// </summary>
        /// <param name="parent">The parent game object whose children will be destroyed</param>
        public static void DestroyAllChildrenImmediate(this GameObject parent)
        {
            parent.transform.DestroyAllChildrenImmediate();
        }

        /// <summary>
        /// Gets all children of a game object as an array of transforms
        /// Shortcut for <see cref="TransformExtensionMethods.GetChildren"/>
        /// </summary>
        /// <param name="parent">The game object whose children should be returned</param>
        /// <returns>An array of children as transforms</returns>
        public static Transform[] GetChildren(this GameObject parent)
        {
            return parent.transform.GetChildren();
        }

        /// <summary>
        /// Get the count of all active children in a game object.
        /// Only considers the activity state of the children, activity of parents is ignored.
        /// Shortcut for <see cref="TransformExtensionMethods.GetActiveChildCount"/>
        /// </summary>
        /// <param name="parent">The parent game object</param>
        /// <returns>The count of active children</returns>
        public static int GetActiveChildCount(this GameObject parent)
        {
            return parent.transform.GetActiveChildCount();
        }

        /// <summary>
        /// Sets the layer for the given gameobject and all childs
        /// </summary>
        /// <param name="parent">The parent game object</param>
        /// <param name="layer">The layer to apply</param>
        public static void SetLayerRecursive(this GameObject parent, int layer)
        {
            parent.layer = layer;
            foreach (Transform childTrans in parent.transform.GetComponentsInChildren<Transform>(true))
            {
                childTrans.gameObject.layer = layer;
            }
        }
    }
}
