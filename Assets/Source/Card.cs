using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    
    public Image face;

    private Animator animator;
    private int index;

    public int Index {
        get {
            return index;
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
        animator.SetTrigger("ShowMe");
        transform.SetAsLastSibling();
    }

    public void Hide() {
        animator.SetTrigger("HideMe");
        transform.SetAsLastSibling();
    }

    public void Win() {
        animator.SetTrigger("Win");
        transform.SetAsLastSibling();
    }
}
