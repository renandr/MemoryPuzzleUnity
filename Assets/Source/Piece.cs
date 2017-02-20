using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour {

    public Image imageTop;
    public Image imageBottom;

    Animator animator;
    bool isShowing = false;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
    }

    public void OnClick() {
        animator.SetTrigger(isShowing?"HideMe":"ShowMe");
        isShowing = !isShowing;
        Debug.Log(isShowing);
    }
}
