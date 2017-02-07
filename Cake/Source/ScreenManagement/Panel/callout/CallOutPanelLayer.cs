using System;
using GGS.CakeBox.Logging;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// The layer view for the call outs. It ensures that only one call out
    /// is displayed at a time.
    /// </summary>
    public class CallOutPanelLayer : PanelLayer
    {
        private OpenScreenVO currentVO;

        protected override void LayerStart()
        {
            base.LayerStart();
            if (Id != CommonLayerIds.CallOuts)
            {
                GGLog.LogError("Wrong PanelLayerType (" + Id + ") for the CallOutPanelLayerView", ScreenSystem.LogType);
            }
        }

        public override void ShowScreen(OpenScreenVO screenVO, Action<IScreen, OpenScreenVO> callback = null, bool forceEnqueue = false)
        {
            if (currentVO != null)
            {
                CloseScreen(currentVO);
            }

            currentVO = screenVO;

            base.ShowScreen(screenVO, callback);
        }

        public override void CloseScreen(OpenScreenVO screenVO, bool animate = true)
        {
            if (screenVO == currentVO)
            {
                currentVO = null;
            }

            base.CloseScreen(screenVO, animate);
        }
    }
}