using UnityEngine;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// The view that handles one specific layer, it should decide how it's gonna load/instantiate and unload/destroy the panels
    /// Currently it's just loading the panels from resources.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class PanelLayer : ALayer
    {

        protected override void LayerStart()
        {
            ScreenSystem.PanelAddedToModel+=AddPanel;
            ScreenSystem.PanelRemovedFromModel+=RemovePanel;
        }

        protected override void OnPanelDestroy()
        {
            ScreenSystem.PanelAddedToModel-=AddPanel;
            ScreenSystem.PanelRemovedFromModel+=RemovePanel;
        }

        private void AddPanel(OpenPanelVO panel)
        {
            if (panel.ScreenLayer == Id)
            {
                ShowScreen(panel);
            }
        }

        private void RemovePanel(OpenPanelVO panel)
        {
            if (panel.ScreenLayer == Id)
            {
                CloseScreen(panel);
            }
        }

        public void ResourcePanelUpdateConfig(float panelSize)
        {
            var rect = GetComponent<RectTransform>();
            rect.offsetMax = new Vector2(rect.offsetMax.x, panelSize);
        }

    }
}