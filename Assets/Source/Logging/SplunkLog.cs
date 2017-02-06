using UnityEngine;

namespace GGS.CakeBox.Logging
{
    /// <summary>
    /// Container for splunk logging data.
    /// Use this as base class for game specific splunk logs and pass it to logging functions of GGLog.
    /// </summary>
    public abstract class SplunkLog
    {
        private const string LoggingUrl = "https://logging.goodgamestudios.com";

        private const string KeyEventId = "eventId";
        private const string KeySuberrorId = "subErrorId";
        private const string KeyErrorText = "errorText";

        protected WWWForm LogData { get; private set; }

        private int eventID;

        private int subErrorID;

        private string message;

        /// <summary>
        /// Creates a new splunk log
        /// </summary>
        /// <param name="eventID">The event ID</param>
        /// <param name="subErrorID">The sub error ID</param>
        /// <param name="message">The slpunk log message, optional</param>
        protected SplunkLog(int eventID, int subErrorID, string message = "")
        {
            this.eventID = eventID;
            this.subErrorID = subErrorID;
            this.message = message;

            LogData = new WWWForm();

            LogData.AddField(KeyEventId, eventID.ToString());
            LogData.AddField(KeySuberrorId, subErrorID.ToString());

        }

        /// <summary>
        /// Adds splunk log data to a splunk log
        /// </summary>
        /// <param name="key">The data key</param>
        /// <param name="value">The data value</param>
        public void AddData(string key, string value)
        {
            LogData.AddField(key, value);
        }

        /// <summary>
        /// Sends the Splunk log message.
        /// Automatically invoked by GGLog when the splunk log is used as parameter in a logging method.
        /// </summary>
        /// <param name="fallbackMessage">The fallback message to be used in case no message was specified before</param>
        public void Send(string fallbackMessage)
        {
            if (string.IsNullOrEmpty(message))
            {
                LogData.AddField(KeyErrorText, eventID + "/" + subErrorID + ": " + fallbackMessage);
            }
            else
            {
                LogData.AddField(KeyErrorText, eventID + "/" + subErrorID + ": " + message);
            }

            new WWW(LoggingUrl, LogData);
        }
    }
}