using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Piece PieceTemplate;
    public float ProportionalPadding = .1f;

    private RectTransform canvasRect;
    private RectTransform myRectTransform;

    private readonly List<Piece> allPieces = new List<Piece>();

    
    private void Awake () {
        myRectTransform = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void Start() {
        for (int pieceIndex = 0; pieceIndex < 20; pieceIndex++) {
            //AddPiece();
        }
    }

    public void AddPiece() {
        var n = Instantiate(PieceTemplate);
        n.gameObject.transform.SetParent(transform, false);
        allPieces.Add(n);

        UpdatePieceGrid();
    }

    private void UpdatePieceGrid() {
        Vector2 gridSize = GetSquariestGridSizes(allPieces.Count);
        Vector2 pieceSize = new Vector2(canvasRect.sizeDelta.x / gridSize.x, myRectTransform.sizeDelta.y / gridSize.y);
        
        int yCount=0, xCount = 0;
        for (int pieceIndex = 0; pieceIndex < allPieces.Count; pieceIndex++) {
            Piece piece = allPieces[pieceIndex];
            
            piece.GetComponent<RectTransform>().sizeDelta = pieceSize*(1-ProportionalPadding);

            piece.transform.localPosition = new Vector3(
                (pieceSize.x * xCount) + pieceSize.x * .5f,
                (-pieceSize.y * yCount)- pieceSize.y * .5f,
                piece.transform.localPosition.z
                );

            bool lineFinished = (xCount == gridSize.x-1);
            xCount = lineFinished ? 0 : xCount + 1;
            yCount = lineFinished ? yCount + 1 : yCount;
        }
    }

    private static Vector2 GetSquariestGridSizes(int amountOfPieces) {
        //get the nearest smaller square for a grid size
        float amountSqrt = Mathf.Sqrt(amountOfPieces);
        int baseSquareSide = (int)Mathf.Floor(amountSqrt);
        var baseSize = new Vector2(baseSquareSide, baseSquareSide);

        // increase the grid size to acomodate pieces in case there's no square grid possible
        baseSize.y += (amountSqrt > baseSquareSide) ? 1 : 0;
        baseSize.x += (amountSqrt % baseSquareSide >= .5) ? 1 : 0;
        
        return baseSize;
    }

}
