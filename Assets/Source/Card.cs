using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    
    public Image face;

    private Animator animator;
    private int index;
    private bool isShowing;

    public int Index {
        get {
            return index;
        }
    }
    public bool IsFacingBack {
        get {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("Iddle");
        }
    }
    
    void Start() {
        animator = GetComponent<Animator>();
    }

    public void SetFace(Sprite sprite, int index) {
        this.index = index;
        face.sprite = sprite;
    }

    public void OnClick() {
        Gameplay.CardClicked(this);
    }

    public void Show() {
        if (!isShowing) {
            animator.SetTrigger("ShowMe");
            transform.SetAsLastSibling();
            isShowing = true;
        }
    }

    public void Hide(bool animate = true) {
        if (animate) {
            if (isShowing) {
                animator.SetTrigger("HideMe");
                transform.SetAsLastSibling();
            }
        }else {
            animator.Play("Iddle");
            ResetTriggers();
        }
        isShowing = false;
    }

    public void Win() {
        animator.SetTrigger("Win");
        transform.SetAsLastSibling();
    }

    private void ResetTriggers() {
        animator.ResetTrigger("HideMe");
        animator.ResetTrigger("ShowMe");
        animator.ResetTrigger("Win");
    }
}
