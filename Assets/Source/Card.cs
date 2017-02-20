using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    
    public Image face;

    private Animator animator;
    private bool isShowing = false;
    private int index;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void SetFace(Sprite sprite, int index) {
        this.index = index;
        face.sprite = sprite;
    }

    public void OnClick() {
        animator.SetTrigger(isShowing?"HideMe":"ShowMe");
        isShowing = !isShowing;
        transform.SetAsLastSibling();
    }
}
