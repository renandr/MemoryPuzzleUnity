using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    public static Camera Camera;
    public Camera MainCamera;

    public Texture2D source;
    public GameObject spritesRoot;
    public Piece templatePiece;
    public static List<List<Piece>> slots;

    private static int iTotal = 4;
    private static int jTotal = 4;
    
    private static int pieceWidth;
    private static int pieceHeight;


    private static Piece selectedPiece;

    public static Piece SelectedPiece{
        set{
            if (value) {
                followerPiece.transform.SetAsLastSibling();
                value.transform.SetAsLastSibling();
            }
            followerPiece.SetActive(value);
            selectedPiece = value;
        }
        get{
            return selectedPiece;
        }
    }

    private static GameObject followerPiece;

    void Start () {
        Camera = MainCamera;
        Croppy();
    }

    private void Croppy(){
        slots = new List<List<Piece>>();
        pieceWidth = source.width / iTotal;
        pieceHeight = source.height / jTotal;

        Debug.Log(source.width + "x" + source.height);
        Debug.Log(pieceWidth + "x" + pieceHeight);

        followerPiece = new GameObject();
        followerPiece.gameObject.transform.SetParent(spritesRoot.transform, false);
        followerPiece.name = "Follower";
        Texture2D texture = new Texture2D(pieceWidth, pieceHeight);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        var image = followerPiece.AddComponent<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0,0,pieceWidth, pieceHeight),new Vector2(0,0), 1);
        followerPiece.GetComponent<RectTransform>().sizeDelta = new Vector2(pieceWidth, pieceHeight);
        //followerPiece.GetComponent<RectTransform>().pivot = new Vector2(0,0);

        for (int i = 0; i < iTotal; i++)
        {
            var slotCollumn = new List<Piece>(jTotal*2);
            slots.Add(slotCollumn);
            for (int j = 0; j < jTotal; j++)
            {
                var n = Instantiate(templatePiece);
                slotCollumn.Add(n);
                n.SetPieceImage(Sprite.Create(source, new Rect(i * pieceWidth, j * pieceHeight, pieceWidth, pieceHeight),
                    new Vector2(0, 0), 1), i, j);
                n.name = "Piece " + i +" "+ j;
                n.gameObject.transform.SetParent(spritesRoot.transform, false);
                
            }
        }
        for (int i = 0; i < iTotal; i++) {
            var slotCollumn = new List<Piece>(jTotal * 2);
            slots.Add(slotCollumn);
        }
    }
    
	void Update () {
        if (selectedPiece)
        {
            followerPiece.transform.localPosition = GetGridSnappedPosition(selectedPiece.gameObject.transform.localPosition);
        }
	}

    internal static void SetValidPosition(Piece piece)
    {
        slots[piece.currentGridX][piece.currentGridY] = null;
        int newPosX = (int)Math.Round(piece.transform.localPosition.x / pieceWidth);
        int newPosY = (int)Math.Round(piece.transform.localPosition.y / pieceHeight);
        
        if (slots[newPosX][newPosY] == null) {
            piece.transform.localPosition = GetGridSnappedPosition(piece.gameObject.transform.localPosition);
        } else {
            piece.transform.localPosition = GetGridSnappedPosition(GetPositionForSlot(piece.currentGridX, piece.currentGridY));
        }
        slots[newPosX][newPosY] = piece;
    }
    
    private static Vector3 GetPositionForSlot(int posX, int posY) {
        return new Vector3(posX * pieceWidth, posY*pieceHeight);
    }
    private static Vector3 GetGridSnappedPosition(Vector3 objLocalPosition)
    {
        Debug.Log("Math.Floor(" + objLocalPosition.x + " / " + pieceWidth + ")[" + (float)Math.Floor(objLocalPosition.x / pieceWidth) + "]" + " * " + pieceWidth + " = " + ((float)Math.Floor(objLocalPosition.x / pieceWidth) * pieceWidth));
        return new Vector3(
            ((float)Math.Round(objLocalPosition.x / pieceWidth) * pieceWidth),
            ((float)Math.Round(objLocalPosition.y / pieceHeight) * pieceHeight),
            0
            );
    }
}
