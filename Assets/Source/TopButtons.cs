using UnityEngine;
using UnityEngine.UI;

public class TopButtons : MonoBehaviour {

    public Button ReplayButton;
    public Button PauseButton;
    public GameObject PauseButtonGO;
    public Button PlayButton;
    public GameObject PlayButtonGO;


    public void EnableControls(bool enabled) {
        ReplayButton.interactable = PauseButton.interactable = PlayButton.interactable = enabled;
    }

    public void TogglePlay(bool showPlay) {
        PlayButtonGO.SetActive(showPlay);
        PauseButtonGO.SetActive(!showPlay);
    }

}
