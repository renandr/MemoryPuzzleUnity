namespace GGS.ScreenManagement
{
    /// <summary>
    /// All call outs have the same ScreenPropertiesVO, containg 
    /// the calling CallOutComponent.
    /// </summary>
    public class CallOutScreenProperties : IScreenPropertiesVO
    {
        public ACallOutComponent CallOutComponent { get; set; }
    }
}