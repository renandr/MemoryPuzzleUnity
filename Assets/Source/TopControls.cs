using UnityEngine;
using UnityEngine.UI;

public class TopControls : MonoBehaviour {

    public Text TimerText;
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

    public float Time {
        set {
            TimerText.text = string.Format("- {0:00}:{1:00}", (int)(value / 60), (int)(value % 60));
        }
    }

    public void ShowLose() {
        TimerText.text = "YOU LOSE";
    }

    public void ShowWin() {
        TimerText.text = "YOU WIN!";
    }


}
