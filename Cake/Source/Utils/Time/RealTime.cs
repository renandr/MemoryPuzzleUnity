using System;

namespace GGS.CakeBox.Utils
{
    /// <summary>
    /// Functions providing access to real time values
    /// </summary>
    public static class RealTime
    {
        private static readonly DateTime startingTime = DateTime.UtcNow;

        /// <summary>
        /// Gets the real time since app start in seconds
        /// Replacement for Unity's Time.realtimeSinceStartup which doesn't work reliably on all platforms.
        /// </summary>
        public static float RealTimeSinceStartup
        {
            get
            {
                return (float) DateTime.UtcNow.Subtract(startingTime).TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the real time since s specified date
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>Time in seconds since specified date</returns>
        public static float RealTimeSinceDate(DateTime date)
        {
            return (float)DateTime.UtcNow.Subtract(date).TotalSeconds;
        }
    }
}