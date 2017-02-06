using UnityEngine;
using System.Runtime.InteropServices;

public class IOSKeyChain {


	private static string m_lastError = "General error.";

	[DllImport ("__Internal")]
	private static extern string _Get (string service, string account);

	[DllImport ("__Internal")]
	private static extern bool _Set (string password, string service, string account);

	[DllImport ("__Internal")]
	private static extern bool _Delete (string service, string account);

	/// <summary>
	/// The last error that has occured.
	/// </summary>
	/// <value>The last error as string.</value>
	public static string LastError {get{return m_lastError;}}

	/// <summary>
	/// Set data to keychain.
	/// </summary>
	/// <returns><c>true</c>, if data was set, <c>false</c> otherwise.</returns>
	/// <param name="service">Service. This is usually your app name.</param>
	/// <param name="key">Key to identify your data.</param>
	/// <param name="value">Value to store.</param>
	public static bool Set(string service, string key, string value)
	{
		m_lastError = null;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (_Set(value,service,key))
				return true;
			else
			{
				m_lastError = "Unable to set value to keychain!";
			}
		}
		else
		{
			m_lastError = "Keychain is not available on non-iOS platforms!";
		}
		return false;
	}

	/// <summary>
	/// Gets data from keychain.
	/// </summary>
	/// <returns>Value stored as string or empty string if no data was found.</returns>
	/// <param name="service">Service. This is usually your app name.</param>
	/// <param name="key">Key that identifies the data you want to get.</param>
	public static string  Get(string service, string key)
	{
		m_lastError = null;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			return _Get(service,key);
		else
		{
			m_lastError = "Keychain is not available on non-iOS platforms!";
		}
		if (m_lastError == null)
			m_lastError = "Key "+key+" not found from keychain!";
		return "";
	}

	/// <summary>
	/// Delete the specified key from service.
	/// </summary>
	/// <returns><c>true</c>, if data was removed, <c>false</c> otherwise.</returns>
	/// <param name="service">Service. This is usually your app name.</param>
	/// <param name="key">Key that identifies the data you want to delete.</param>
	public static bool Delete(string service, string key)
	{
		m_lastError = null;
		if (Application.platform == RuntimePlatform.IPhonePlayer)
			return _Delete(service,key);
		else
		{
			m_lastError = "Keychain is not available on non-iOS platforms!";
		}
		if (m_lastError == null)
			m_lastError = "Key "+key+" not found from keychain!";
		return false;
	}

}
