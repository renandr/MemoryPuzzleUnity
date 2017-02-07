using UnityEngine.UI;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Extention methods for the ScreenViewState for adding and reading values for specific UI widgets
    /// </summary>
    public static class DialogStateExtensions
    {
        /// <summary>
        /// A ScreenViewState extension method that adds a slider state.
        /// </summary>
        /// <param name="dialogState">  The screenViewState to act on. </param>
        /// <param name="slider">   The slider. </param>
        /// <param name="identifier">   The identifier. </param>
        public static void AddSliderState(this DialogState dialogState, Slider slider, string identifier = null)
        {
            string name = identifier ?? slider.name;

            dialogState.AddFloatState(name, slider.value);
        }

        /// <summary>
        /// A ScreenViewState extension method that reads slider state.
        /// </summary>
        /// <param name="dialogState"> The screenViewState to act on. </param>
        /// <param name="slider"> The slider. </param>
        /// <param name="identifier"> The identifier. </param>
        /// <returns>  The slider state.  </returns>
        public static float ReadSliderState(this DialogState dialogState, Slider slider, string identifier = null)
        {
            string name = identifier ?? slider.name;

            return dialogState.ReadFloatState(name);
        }
    }
}