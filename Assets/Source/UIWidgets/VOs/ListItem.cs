using System;
using com.goodgamestudios.warlands.uiWidgets.List;
using GGS.WMO.Shop;
using UnityEngine;

namespace com.goodgamestudios.warlands.uiWidgets.vos
{
    [Serializable]
    public class ListItem
    {
        public int Id;
        public string Name;
        public UIListItem<ShopVO> ListItemTemplate;
//        public UIListItem ListItemTemplate;
    }
}