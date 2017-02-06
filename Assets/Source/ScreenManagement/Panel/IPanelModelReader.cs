namespace GGS.ScreenManagement
{
    /// <summary>
    /// Interface for read-only access to the Panel Model
    /// The Panel Model stores the VO's with information about opening panels
    /// Ideally you shouldn't give the OpenPanelVO for read only access, only the properties
    /// </summary>
    public interface IPanelModelReader : IScreenModelReader
    {
    }
}