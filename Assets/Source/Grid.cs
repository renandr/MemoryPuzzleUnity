using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Card CardTemplate;

    [Range(0, 0.4f)]
    public float ProportionalPadding;

    private RectTransform canvasRect;
    private RectTransform myRectTransform;

    private readonly List<Card> allCards = new List<Card>();

    private void Awake () {
        myRectTransform = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void Start() {
        for (int pieceIndex = 0; pieceIndex < 20; pieceIndex++) {
            //AddPiece();
        }
    }

    public void AddCard() {
        var n = Instantiate(CardTemplate);
        n.gameObject.transform.SetParent(transform, false);
        allCards.Add(n);

        UpdatePieceGrid();
    }

    private void UpdatePieceGrid() {
        Vector2 gridSize = GetBestFitGridSizes(allCards.Count);
        Vector2 pieceSize = new Vector2(canvasRect.sizeDelta.x / gridSize.x, myRectTransform.sizeDelta.y / gridSize.y);
        
        int yCount=0, xCount = 0;
        for (int pieceIndex = 0; pieceIndex < allCards.Count; pieceIndex++) {
            Card piece = allCards[pieceIndex];
            
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

    private static Vector2 GetBestFitGridSizes(int amountOfPieces) {
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
