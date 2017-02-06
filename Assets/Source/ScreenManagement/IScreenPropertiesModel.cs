namespace GGS.ScreenManagement
{
    public interface IScreenModelReader
    {
        IScreenPropertiesVO GetProperties(ScreenIdentifier uiid, int instanceId);

        T GetProperties<T>(ScreenIdentifier uiid, int instanceId);

        bool Contains(ScreenIdentifier screenIdentifier);
    }
}
