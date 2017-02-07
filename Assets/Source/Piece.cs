using System;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private Sprite mySprite;
    void Start()
    {
        
        
    }


    public void SetPieceImage(Sprite newSprite, int i, int j)
    {
        var rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) Debug.LogError("where is the rect?");

        mySprite = newSprite;

        var sr = gameObject.GetComponent<Image>();
        sr.sprite = newSprite;
        rectTransform.sizeDelta = new Vector2(newSprite.bounds.size.x*100, newSprite.bounds.size.y*100);
        transform.position = new Vector3(i*rectTransform.sizeDelta.x, j*rectTransform.sizeDelta.y, 0);
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
