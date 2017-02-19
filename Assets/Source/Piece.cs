using UnityEngine;

public class Piece : MonoBehaviour {
    
    
    public Vector2 ReferenceSize {
        get {
            return new Vector2(180, 320);
            //GetComponent<RectTransform>().sizeDelta
        }
    }

	void Awake () {
    }
}
