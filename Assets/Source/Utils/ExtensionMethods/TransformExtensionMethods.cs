using System.Diagnostics;
using UnityEngine;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// <see href="https://msdn.microsoft.com/en-us/library/bb383977.aspx">C# extension methods</see> for Unity's <see cref="Transform"/>-class
    /// </summary>
    public static class TransformExtensionMethods
    {
        /// <summary>
        /// Destroys the game object belonging to a transform
        /// This is a shortcut method. Call <see cref="GameObjectExtensionMethods.DestroyAllChildrenImmediate"/> instead if the game object is directly available.
        /// </summary>
        /// <param name="transform">The transform of the game object to be destroy</param>
        public static void Destroy(this Transform transform)
        {
            Object.Destroy(transform.gameObject);
        }

        /// <summary>
        /// Utility function to set the name of a transform
        /// This is only compiled into the editor code and does not influence the names in mobile builds.
        /// </summary>
        /// <param name="transform">The transform to be renamed</param>
        /// <param name="name">Name for the transform</param>
        [Conditional("UNITY_EDITOR")]
        public static void SetNameInEditorOnly(this Transform transform, string name)
        {
            transform.name = name;
        }

        /// <summary>
        /// Destroys all children of a transform
        /// </summary>
        /// <param name="parent">The parent transform whose children will be destroyed</param>
        public static void DestroyAllChildren(this Transform parent)
        {
            foreach (Transform t in parent)
            {
                Object.Destroy(t.gameObject);
            }
        }

        /// <summary>
        /// Destroys all children of a transform immediately
        /// Should be used in the editor only, see: http://docs.unity3d.com/ScriptReference/Object.DestroyImmediate.html
        /// </summary>
        /// <param name="parent">The parent transform whose children will be destroyed</param>
        public static void DestroyAllChildrenImmediate(this Transform parent)
        {
            // It's not possible to remove children while iterating over all children so cache them first and then remove them
            Transform[] children = parent.GetChildren();

            for (var i = 0; i < children.Length; i++)
            {
                Object.DestroyImmediate(children[i].gameObject);
            }
        }

        /// <summary>
        /// Gets all children of a transform as an array of transforms
        /// </summary>
        /// <param name="parent">The transform whose children should be returned</param>
        /// <returns>An array of children as transforms</returns>
        public static Transform[] GetChildren(this Transform parent)
        {
            var children = new Transform[parent.childCount];
            var i = 0;
            foreach (Transform child in parent)
            {
                children[i] = child;
                i++;
            }
            return children;
        }

        /// <summary>
        /// Get the count of all active children in a transform.
        /// Only considers the activity state of the children, activity of parents is ignored.
        /// </summary>
        /// <param name="parent">The parent transform</param>
        /// <returns>The count of active children</returns>
        public static int GetActiveChildCount(this Transform parent)
        {
            int count = 0;
            foreach (Transform child in parent)
            {
                if (child.gameObject.activeSelf)
                {
                    count++;
                }
            }
            return count;
        }
    }
}