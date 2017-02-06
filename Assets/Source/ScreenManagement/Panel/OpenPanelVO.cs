using System;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Information about opening panels, such as which layer it should be added to
    /// </summary>
    public class OpenPanelVO : OpenScreenVO
    {
        /// <summary>
        /// Which PanelLayerView should take this panel
        /// </summary>
        public string ScreenLayer;

        public OpenPanelVO(ScreenIdentifier uniqueId, IScreenPropertiesVO properties, string screenLayer)
            : base(uniqueId, properties)
        {
            ScreenLayer = screenLayer;
        }

        public OpenPanelVO(ScreenIdentifier uniqueId, string screenLayer)
            : this(uniqueId, null, screenLayer)
        {
        }

        public void ReplaceContents(OpenPanelVO newVO)
        {
            if (ScreenLayer != newVO.ScreenLayer)
            {
                throw new ArgumentException("You're trying to open an existing panel in a different layer? No can do.");
            }
            base.ReplaceContents(newVO);
        }

    }

}