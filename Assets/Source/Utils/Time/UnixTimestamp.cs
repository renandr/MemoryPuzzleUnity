using System;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Utility class used to provide Unix timestamp related things
    /// </summary>
    public static class UnixTimestamp
    {
        private static readonly DateTime epochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The Unix epoch <see cref="DateTime"/>
        /// </summary>
        public static DateTime UnixEpoch
        {
            get { return epochDateTime; }
        }

        /// <summary>
        /// Seconds since unix epoch
        /// </summary>
        public static int Seconds
        {
            get { return (Int32) (DateTime.UtcNow.Subtract(epochDateTime)).TotalSeconds; }
        }

        /// <summary>
        /// Milliseconds since unix epoch
        /// </summary>
        public static int MilliSeconds
        {
            get { return (DateTime.UtcNow.Subtract(epochDateTime)).Milliseconds; }
        }
    }
}