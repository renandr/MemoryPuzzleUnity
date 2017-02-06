namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// A struct representing the PING of a player account.
    /// Attention: Do not confuse this PING with a network ping. A "PING" in GGS is a set of player account related values:
    /// P = Player ID
    /// I = Instance ID
    /// N = Network ID
    /// G = Game ID
    /// </summary>
    public struct PING
    {
        private const string GNIPDelimiter = "-";

        public readonly int PlayerID;
        public readonly int InstanceID;
        public readonly byte NetworkID;
        public readonly byte GameID;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="playerID">The player ID</param>
        /// <param name="instanceID">The instance ID</param>
        /// <param name="networkID">The network ID</param>
        /// <param name="gameID">The game ID</param>
        public PING(int playerID, int instanceID, byte networkID, byte gameID)
        {
            PlayerID = playerID;
            InstanceID = instanceID;
            NetworkID = networkID;
            GameID = gameID;
        }

        /// <summary>
        /// Converts this <see cref="PING"/> to a JSON string.
        /// </summary>
        /// <returns>
        /// This <see cref="PING"/> as a JSON string.
        /// </returns>
        public string ToJSON()
        {
            return "{p:" + PlayerID + ",i:" + InstanceID + ",n:" + NetworkID + ",g:" + GameID + "}";
        }

        /// <summary>
        /// Converts this <see cref="PING"/> to a "G-N-I-P"-string
        /// </summary>
        /// <returns>This <see cref="PING"/> as a G-N-I-P string</returns>
        public string ToGNIP()
        {
            return GameID + GNIPDelimiter + NetworkID + GNIPDelimiter + InstanceID + GNIPDelimiter + PlayerID;
        }

        #region Overrides of ValueType

        public override string ToString()
        {
            return ToJSON();
        }

        #endregion
    }
}