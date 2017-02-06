namespace GGS.ScreenManagement
{
    /// <summary>
    ///  Delegates for tween animation.
    /// </summary>

    #region Request Delegates
    //the delegate for open or close a screen view
    public delegate void ScreenUIAnimationDelegate(CompleteUIAnimationDelegate uiAnimationCompletedCallback, IScreen screen);
    #endregion

    #region Response Delegates
    // The animation completed callback.
    public delegate void CompleteUIAnimationDelegate(IScreen screen);

    #endregion

    //A generic delegate for the tween components
    public delegate void TweenAnimationDelegate();
}
