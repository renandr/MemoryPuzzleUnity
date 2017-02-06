using System;
using GGS.CakeBox.Logging;

namespace GGS.ScreenManagement
{
    /// <summary>
    /// Identifier for any UI elements of the game, contains the UI's name and the unique identifier
    /// </summary>
    public struct ScreenIdentifier : IEquatable<ScreenIdentifier>
    {
        private const string DefaultUniqueId = "Default";
        public string AssetId { get; private set; }
        public string UniqueId { get; private set; }

        public ScreenIdentifier(string assetId, string uniqueId = DefaultUniqueId)
            : this()
        {
            AssetId = assetId;
            UniqueId = string.IsNullOrEmpty(uniqueId) ? DefaultUniqueId : uniqueId;

            if (uniqueId == DefaultUniqueId)
            {
                switch (assetId)
                {
//                    case DialogConstants.ConfirmationDialog:
//                        GGLog.LogError(assetId + " missing a uniqueId!", ScreenSystem.LogType);
//                        break;
                }
            }
        }

        public override string ToString()
        {
            return AssetId + "_" + UniqueId;
        }

        public bool Equals(ScreenIdentifier other)
        {
            return other.AssetId == AssetId && other.UniqueId == UniqueId;
        }
        public static bool operator ==(ScreenIdentifier a, ScreenIdentifier b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ScreenIdentifier a, ScreenIdentifier b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is ScreenIdentifier && Equals((ScreenIdentifier)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 21;
                if (!string.IsNullOrEmpty(AssetId))
                {
                    hash = hash * 33 + AssetId.GetHashCode();
                }
                if (!string.IsNullOrEmpty(UniqueId))
                {
                    hash = hash * 33 + UniqueId.GetHashCode();
                }
                return hash;
            }
        }
    }
}