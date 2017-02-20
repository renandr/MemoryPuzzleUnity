using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

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
        transform.SetAsLastSibling();
    }
}
