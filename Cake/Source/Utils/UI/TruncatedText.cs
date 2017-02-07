using UnityEngine;
using UnityEngine.UI;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// An extension to Text, which truncates the string and adds "..." to the end.
    /// http://answers.unity3d.com/questions/836642/new-ui-46-how-to-show-ellipsis-for-text-overflow.html
    /// </summary>
    /// <seealso cref="T:UnityEngine.UI.Text"/>
    public class TruncatedText : Text
    {
        private string updatedText = string.Empty;

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            Vector2 extents = rectTransform.rect.size;
            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.Populate(text, settings);

            float scale = extents.x / preferredWidth;
            //text is going to be truncated, 
            //cant update the text directly as we are in the graphics update loop
            if (scale < 1)
            {
                updatedText = text.Substring(0, cachedTextGenerator.characterCount - 4);
                updatedText += "...";
            }
            else
            {
                updatedText = text;
            }
            base.OnPopulateMesh(toFill);
        }

        private void Update()
        {
            if (updatedText != string.Empty && updatedText != text)
            {
                text = updatedText;
            }
        }
    }
}