using System;
using GGS.WMO.UI;
using UnityEngine;

namespace com.goodgamestudios.warlands.uiWidgets.List
{
    [Serializable]
    public abstract class UIListItem<D> : MonoBehaviour where D : AListVO
    {
        public int Id;

        public abstract void PopulateData(D data);

        
    }
}