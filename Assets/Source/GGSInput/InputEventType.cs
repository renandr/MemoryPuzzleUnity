namespace GGS.CakeBox.GGSInput
{
    public enum InputEventType
    {
        None,

        /// <summary>
        /// Fired in the frame you start to do a one finger touch (or start to click a mouse button)
        /// </summary>
        SingleDown,

        /// <summary>
        /// Fired every frame with a one finger touch (or when mouse button is held down)
        /// </summary>
        SinglePressed,

        /// <summary>
        /// Fired in the frame you stop touching with one finger (or release the mouse button)
        /// </summary>
        SingleUp,

        /// <summary>
        /// When dragging the finger/mouse
        /// </summary>
        Drag,

        /// <summary>
        /// When dragging with two fingers
        /// </summary>
        TwoFingerDrag,

        /// <summary>
        /// When dragging with three fingers
        /// </summary>
        ThreeFingerDrag,

        /// <summary>
        /// When pinching
        /// </summary>
        Pinch,

        /// <summary>
        /// When rotating
        /// </summary>
        Rotate
    }
}