using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GGS.CakeBox.Logging;

namespace GGS.GameLocks
{
    /// <summary>
    /// The model holding information about the current active lock conditions
    /// TODO figure out how to take the name of the id"s
    /// </summary>
    public class LockSystemModel : ILockSystemModelReader
    {
        private const string UnnamedString = "unnamed";

        /// <summary>
        /// List of running lock conditions.
        /// </summary>
        private readonly Dictionary<int, List<LockConditionVO>> networkMessages = new Dictionary<int, List<LockConditionVO>>();
        private readonly Dictionary<CustomLockReason, List<string>> customLocks = new Dictionary<CustomLockReason, List<string>>();

        private static bool lastPrintWasEmpty = false;

        public bool PingRequested{ get; set; }

        public LockSystemModel()
        {
            networkMessages = new Dictionary<int, List<LockConditionVO>>();
            customLocks = new Dictionary<CustomLockReason, List<string>>();
        }

        /// <summary>
        /// Returns true if there currently are any custom locks or network locks, false otherwise
        /// </summary>
        public bool HasLocks
        {
            get
            {
                if (customLocks.Count > 0)
                {
                    return true;
                }
                return HasNetworkLocks;
            }
        }

        public bool HasLocksOfType(CustomLockReason reason)
        {
            return customLocks.ContainsKey(reason);
        }


        /// <summary>
        /// Returns true if there are currently any network locks
        /// </summary>
        public bool HasNetworkLocks
        {
            get
            {
                foreach (List<LockConditionVO> list in networkMessages.Values)
                {
                    if (ListHasLocks(list))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private bool ListHasLocks(List<LockConditionVO> list)
        {
            foreach (LockConditionVO vo in list)
            {
                if (vo.LockBehaviourType == GameLockBehaviourType.Lock)
                {
                    return true;
                }
            }
            return false;
        }

        public void Add(int id, LockConditionVO vo)
        {
            if (!networkMessages.ContainsKey(id))
            {
                networkMessages.Add(id, new List<LockConditionVO>{vo});
            }
            else
            {
                networkMessages[id].Add(vo);
            }
            PrintStatus("Added Network Lock: " + id);//GetMessageTypeName(id));
        }

        public void Add(CustomLockReason reason, string customString = UnnamedString)
        {
            if (!customLocks.ContainsKey(reason))
            {
                customLocks.Add(reason, new List<string>
                {
                    customString
                });
            }
            else
            {
                customLocks[reason].Add(customString);
            }
            PrintStatus("Added Custom Lock: " + reason + (customString == UnnamedString ? "" : (", " + customString)));
        }

        public bool ContainsCustomLock(CustomLockReason reason, string lockString)
        {
            if (!customLocks.ContainsKey(reason))
            {
                return false;
            }
            foreach (KeyValuePair<CustomLockReason, List<string>> cLock in customLocks)
            {
                if (cLock.Key == reason)
                {
                    foreach (string cLockStr in cLock.Value)
                    {
                        if (cLockStr == lockString)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool ContainsNetworkLock(int commandId)
        {
            return networkMessages.ContainsKey(commandId);
        }

        public int RemoveCustomLock(CustomLockReason reasonToRemove, string customString = UnnamedString)
        {
            if (customLocks.ContainsKey(reasonToRemove))
            {
                customLocks[reasonToRemove].Remove(customString);
                int ret = customLocks[reasonToRemove].Count;
                if (customLocks[reasonToRemove].Count == 0)
                {
                    customLocks.Remove(reasonToRemove);
                }
                PrintStatus("Removed Custom Lock: " + reasonToRemove + (customString == UnnamedString ? "" : (", " + customString)));
                return ret;
            }
            else
            {
               return -1;
            }
        }

        public void RemoveAllCustomLocks()
        {
            if (customLocks != null)
            {
                customLocks.Clear();
            }

        }

        public void RemoveAllNetworkLocks()
        {
            if (networkMessages != null)
            {
                networkMessages.Clear();
            }
        }

        public LockConditionVO RemoveNetworkLock(int idToRemove)
        {
            if (networkMessages.ContainsKey(idToRemove))
            {
                List<LockConditionVO> list = networkMessages[idToRemove];
                LockConditionVO lockToReturn = list[0];
                networkMessages[idToRemove].Remove(lockToReturn);
                if (list.Count == 0)
                {
                    networkMessages.Remove(idToRemove);
                }
                PrintStatus("Removed Network Lock: " + idToRemove);//TODO GetMessageTypeName(idToRemove));
                return lockToReturn;
            }
            else
            {
                throw new ArgumentException("Trying to remove unexisting lock " + idToRemove);
            }
        }

        [Conditional("DEBUG")]
        public void PrintStatus(string str = "")
        {
            if (!lastPrintWasEmpty || HasLocks)
            {
                str += (string.IsNullOrEmpty(str) ? "" : "\n") + ToString();
                GGLog.Log(str, LockSystem.LogType);
            }
            lastPrintWasEmpty = !HasLocks;
        }

        /// <summary>
        /// Prints the current active locks.
        /// </summary>
        /// <returns>Multiline string.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            bool hasLocks = HasLocks;
            if (hasLocks)
            {
                builder.AppendLine("System is locked:");
                builder.AppendFormat("Active network locks {0}:\n", networkMessages.Count);
                foreach (int lockId in networkMessages.Keys)
                {
                    builder.AppendFormat("- {0} ({1}) {2}\n",/*TODO GetMessageTypeName(lockId)*/ lockId, ListHasLocks(networkMessages[lockId]) ? " locked" : " free");
                }
                builder.AppendFormat("Active custom locks {0}:\n", customLocks.Count);
                foreach (CustomLockReason lockReason in customLocks.Keys)
                {
                    builder.AppendFormat("- {0}: {1}\n", lockReason, customLocks[lockReason].Count);
                    foreach (string lockString in customLocks[lockReason])
                    {
                        builder.AppendFormat("-- {0}: {1}\n", lockReason, lockString);
                    }
                }
            }
            else
            {
                builder.AppendFormat("No active locks");
            }

            return builder.ToString();
        }

//        TODO private string GetMessageTypeName(int messageId)
//        {
//            if (messageId >= LohmClientConstants.FirstCoreCommand)
//            {
//                return ((WLMobileCoreCommand)messageId).ToString();
//            }
//            else
//            {
//                return ((DataMobileCommandNetwork.DataMobileCommandType)messageId).ToString();
//            }
//
//        }
    }
}