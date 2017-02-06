namespace GGS.GameLocks
{
    /// <summary>
    /// It's an enum because it's more readable in the signal definition
    /// Default should be locked, if you're requesting something it makes more sense to lock
    /// </summary>
    public enum GameLockBehaviourType
    {
        Lock,
        IgnoreLock
    }
}