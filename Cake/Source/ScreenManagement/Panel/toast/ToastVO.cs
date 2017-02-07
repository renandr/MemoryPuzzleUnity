using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// The VO class for toast messages. 
    /// </summary>
    public class ToastVO
    {
        /// <summary>
        /// Defines with icon type should be shown. The icon graphic gets looked up in the 
        /// ToastIconConfiguration ScriptableObject.
        /// This is only used if <see cref="Icon"/> is null.
        /// </summary>
        public ToastIconType IconType { get; set; }

        /// <summary>
        /// The icon to be used.
        /// If this is null the game will get the icon defined by <see cref="IconType"/>.
        /// </summary>
        public Sprite Icon { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// If your toast should be clickable set this delegate.
        /// </summary>
        public System.Action ClickCallback { get; set; }

        /// <summary>
        /// Creates a new toast VO with an icon specified with the <see cref="ToastIconType"/> enum.
        /// </summary>
        /// <param name="titleText">The title text</param>
        /// <param name="contentText">The content text</param>
        /// <param name="icon">The icon type (enum)</param>
        /// <param name="clickCallback">The callback</param>
        public ToastVO(string titleText, string contentText, ToastIconType icon = ToastIconType.None,
            System.Action clickCallback = null)
        {
            Title = titleText;
            Content = contentText;
            IconType = icon;
            ClickCallback = clickCallback;
        }

        /// <summary>
        /// Creates a new toast VO with a custom icon sprite.
        /// </summary>
        /// <param name="titleText">The title text</param>
        /// <param name="contentText">The content text</param>
        /// <param name="icon">The icon sprite</param>
        /// <param name="clickCallback">The callback</param>
        public ToastVO(string titleText, string contentText, Sprite icon,
            System.Action clickCallback = null)
        {
            Title = titleText;
            Content = contentText;
            IconType = ToastIconType.None;
            Icon = icon;
            ClickCallback = clickCallback;
        }
    }
}