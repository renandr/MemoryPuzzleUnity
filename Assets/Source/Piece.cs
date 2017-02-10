using System;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour {

    private Vector3 screenPoint;
    private Vector3 offset;
    private Sprite mySprite;

    RectTransform rectTransform;
    public int currentGridX = -1;
    public int currentGridY = -1;

    void Start()
    {
        
        
    }


    public void SetPieceImage(Sprite newSprite, int i, int j)
    {
        currentGridX = i;
        currentGridY = j;
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) Debug.LogError("where is the rect?");

        mySprite = newSprite;

        var sr = gameObject.GetComponent<Image>();
        sr.sprite = newSprite;
        rectTransform.sizeDelta = new Vector2(newSprite.bounds.size.x, newSprite.bounds.size.y);
        transform.position = new Vector3(i*rectTransform.sizeDelta.x, j*rectTransform.sizeDelta.y, 0);
        Main.SetValidPosition(this);
    }
   
    public void UI_OnMouseDown(){
        screenPoint = Main.Camera.WorldToScreenPoint(transform.position);
        offset = transform.position - Main.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Main.SelectedPiece = this;
    }

    public void UI_OnMouseDrag(){
        //Debug.Log("mouse "+Input.mousePosition);
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Main.Camera.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;

        //Debug.Log(rectTransform.localPosition + " dragging at position " + transform.position);

    }

    public void UI_OnMouseUp()
    {
        Main.SetValidPosition(this);
        Main.SelectedPiece = null;
    }


}
