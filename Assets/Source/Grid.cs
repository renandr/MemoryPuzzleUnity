using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    [Range(0, 0.4f)]
    public float ProportionalPadding;

    private RectTransform canvasRect;
    private RectTransform myRectTransform;

    private readonly List<GameObject> allItems = new List<GameObject>();

    private void Awake () {
        myRectTransform = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void Start() {
        
    }
    
    public void AddItem(GameObject item) {
        item.gameObject.transform.SetParent(transform, false);
        allItems.Add(item);
        UpdateGridSpacing();
    }

    private void UpdateGridSpacing() {
        Vector2 gridSize = GetBestFitGridSizes(allItems.Count);
        Vector2 itemSize = new Vector2(canvasRect.sizeDelta.x / gridSize.x, myRectTransform.sizeDelta.y / gridSize.y);
        
        int yCount=0, xCount = 0;
        for (int i = 0; i < allItems.Count; i++) {
            GameObject item = allItems[i];

            item.GetComponent<RectTransform>().sizeDelta = itemSize * (1-ProportionalPadding);

            item.transform.localPosition = new Vector3(
                (itemSize.x * xCount) + itemSize.x * .5f,
                (-itemSize.y * yCount)- itemSize.y * .5f,
                item.transform.localPosition.z
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
