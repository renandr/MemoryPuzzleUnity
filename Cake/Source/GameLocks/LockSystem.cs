using GGS.CakeBox.Logging;
using GGS.CakeBox.Utils;

namespace GGS.GameLocks
{
    public static class LockSystem
    {
        internal static LockSystemModel ModelWritter { get; set; }
        public static ILockSystemModelReader Model { get; private set; }

        public static event GenericEvent LocksUpdated;

        internal static string LogType = "LockSystem";

        public static void Init()
        {
            Model = ModelWritter = new LockSystemModel();
            GGLog.AddLogType(LogType);
        }

        public static void CustomLock(CustomLockReason customLockReason, string customString, bool isAdd)
        {
            if (isAdd)
            {
                ModelWritter.Add(customLockReason, customString);
            }
            else
            {
                ModelWritter.RemoveCustomLock(customLockReason, customString);
            }
            LocksUpdated.Fire();
        }

        public static void NotifyLocksUpdated()
        {
            LocksUpdated.Fire();
        }
    }
}
