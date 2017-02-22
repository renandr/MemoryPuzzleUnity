using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the view states of the card
/// </summary>
public class Card : MonoBehaviour {

    [Tooltip("The image assigned by the gameplay.")]
    public Image face;

    private Animator animator;

    /// <summary>
    /// Index of the face displayed
    /// </summary>
    private int index;

    /// <summary>
    /// Used mainly for avoiding edge cases, like when the game finishes and the animation is still running
    /// </summary>
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
            animator.Play("Iddle");
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
