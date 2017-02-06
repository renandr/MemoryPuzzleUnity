using System.Collections.Generic;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// The Panel Model stores the VO's with properties and information about opening panels
    /// </summary>
    public class PanelModel : IPanelModelReader
    {
        private readonly Dictionary<ScreenIdentifier, OpenPanelVO> panels;

        public PanelModel()
        {
            panels = new Dictionary<ScreenIdentifier, OpenPanelVO>();
        }

        public IScreenPropertiesVO GetProperties(ScreenIdentifier uiid, int instanceId)
        {
            if (panels.ContainsKey(uiid))
            {
                return panels[uiid].Properties;
            }
            return null;
        }

        public T GetProperties<T>(ScreenIdentifier uiid, int instanceId)
        {
            IScreenPropertiesVO props = GetProperties(uiid, instanceId);
            if (props == null)
            {
                return default(T);
            }
            return (T)props;
        }

        /// <summary>
        /// Only add this to the reader interface if it's super necessary
        /// </summary>
        public OpenPanelVO Get(ScreenIdentifier uiid)
        {
            return panels.ContainsKey(uiid) ? panels[uiid] : null;
        }

        public void Add(OpenPanelVO dialogProperties)
        {
            panels.Add(dialogProperties.Id, dialogProperties);
        }

        public bool Contains(ScreenIdentifier uiid)
        {
            return panels.ContainsKey(uiid);
        }

        public void ReplaceContents(OpenPanelVO openPanelVO)
        {
            panels[openPanelVO.Id].ReplaceContents(openPanelVO);
        }

        public OpenPanelVO Remove(ScreenIdentifier screenId)
        {
            var panelVO = Get(screenId);
            if (panelVO != null)
            {
                panels.Remove(screenId);
                return panelVO;
            }
            return null;
        }

        /// <summary>
        /// Remove by Asset Name
        /// </summary>
        /// <param name="panelAssetId"></param>
        /// <returns>List or empty list if nothing found</returns>
        public List<OpenPanelVO> RemoveByType(string panelAssetId)
        {
            List<OpenPanelVO> itemsToRemove = new List<OpenPanelVO>();

            foreach (OpenPanelVO panel in panels.Values)
            {
                if (panel.Id.AssetId == panelAssetId)
                {
                    itemsToRemove.Add(panel);
                }
            }

            for (int i = 0; i < itemsToRemove.Count; i++)
            {
                panels.Remove(itemsToRemove[i].Id);
            }

            return itemsToRemove;
        }

    }
}