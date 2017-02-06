using System;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using GGS.CakeBox.Preferences;
using GGS.CakeBox.DeviceInfo;
using GGS.CakeBox.GGSInput;
using GGS.CakeBox.Logging;
using GGS.CakeBox.Utils;

/// <summary>
/// Class for testing utils
/// </summary>
public class UtilTest : MonoBehaviour
{
    [SerializeField]
    private GameObject cube;

    private InputEventData lastInputEventData;

	// Use this for initialization
	private void Start () {
        // Loging Test
	    const string testLog = "TestLogType";
        GGLog.AddLogType(testLog);
        GGLog.Log("test", testLog);

        // Basic Utils Tests
        List<int> bla = new List<int>{1, 5, 3, 7};
	    GGLog.Log(bla.ContentToString(), testLog);

	    GGLog.Log("helloWorld".FastEndsWith("helloWorld").ToString(), testLog);

        // Device Test
	    DeviceConfiguration deviceConfig = ScriptableObject.CreateInstance<DeviceConfiguration>();
        Device.Initialize(deviceConfig);
        GGLog.Log(Device.ToString(), testLog);

        // Prefs Test
        GGLog.Log("last execution time: " + Prefs.GetString("lastExecutionTime"), testLog);
        Prefs.SetString("lastExecutionTime", DateTime.Now.ToString(CultureInfo.CurrentCulture));
	    Prefs.Save();

        MapInput();
	}

    #region Input Event Handling

    void LateUpdate()
    {
        InputManager.HandleEvents();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 500, 100), "Last Input Event: "+ lastInputEventData.EventType +" Screenpos: " + lastInputEventData.ScreenPosition);
    }

    private void MapInput()
    {
        InputManager.OnSingleDown = OnSingleDown;
        InputManager.OnSinglePressed = OnSinglePressed;
        InputManager.OnSingleUp = OnSingleUp;
        InputManager.OnDrag = OnDrag;
        InputManager.OnTwoFingerDrag = OnTwoFingerDrag;
        InputManager.OnThreeFingerDrag = OnThreeFingerDrag;
        InputManager.OnPinch = OnPinch;
        InputManager.OnRotate = OnRotate;
    }

    private void OnSingleDown(InputEventData data)
    {
        lastInputEventData = data;
    }

    private void OnSinglePressed(InputEventData data)
    {
        lastInputEventData = data;
    }

    private void OnSingleUp(InputEventData data)
    {
        lastInputEventData = data;
        cube.GetComponent<MeshRenderer>().sharedMaterial.color = Color.white;
    }

    private void OnDrag(InputEventData data)
    {
        lastInputEventData = data;
        Camera.main.transform.Translate(data.Vector3Value * 10f);
        cube.GetComponent<MeshRenderer>().sharedMaterial.color = Color.yellow;
    }

    private void OnTwoFingerDrag(InputEventData data)
    {
        lastInputEventData = data;
        Camera.main.transform.Translate(data.Vector3Value * 3f);
        cube.GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
    }

    private void OnThreeFingerDrag(InputEventData data)
    {
        lastInputEventData = data;
        Camera.main.transform.Translate(data.Vector3Value * 3f);
        cube.GetComponent<MeshRenderer>().sharedMaterial.color = Color.blue;
    }

    private void OnPinch(InputEventData data)
    {
        lastInputEventData = data;
    }

    private void OnRotate(InputEventData data)
    {
        lastInputEventData = data;
    }

    #endregion
}
