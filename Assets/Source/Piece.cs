using System;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;

    public void SetPieceImage(Sprite newSprite)
    {
        var sr = gameObject.GetComponent<Image>();
        sr.sprite = newSprite;
    }

    void Start(){
        Debug.Log("iiii");
    }


    void Update(){

    }
    
    public void UI_OnMouseDown(){
        screenPoint = Main.Camera.WorldToScreenPoint(transform.position);
        offset = transform.position - Main.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    public void UI_OnMouseDrag(){
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Main.Camera.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    
}
