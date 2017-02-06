namespace GGS.GameLocks
{
    public interface ILockSystemModelReader
    {
        bool ContainsCustomLock(CustomLockReason loadingAsset, string assetId);
        bool HasLocks { get; }
        bool ContainsNetworkLock(int messageId);
    }
}