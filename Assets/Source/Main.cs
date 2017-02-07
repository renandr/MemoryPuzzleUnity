using UnityEngine;

public class Main : MonoBehaviour {
    public static Camera Camera;

    public Camera MainCamera;

    public Texture2D source;
    public GameObject spritesRoot;
    public Piece templatePiece;

    void Start () {
        Camera = MainCamera;
        Croppy();
    }

    private void Croppy(){
        int iTotal = 4;
        int jTotal = 4;
        int pieceWidth = source.width / iTotal;
        int pieceHeight = source.height / jTotal;

        Debug.Log(source.width + "x" + source.height);
        Debug.Log(pieceWidth + "x" + pieceHeight);

        for (int i = 0; i < iTotal; i++)
        {
            for (int j = 0; j < jTotal; j++)
            {
                var n = Instantiate(templatePiece);
                n.SetPieceImage(Sprite.Create(source, new Rect(i * pieceWidth, j * pieceHeight, pieceWidth, pieceHeight),
                    new Vector2(0, 0)), i, j);
                n.name = "Piece " + i +" "+ j;
                n.gameObject.transform.SetParent(spritesRoot.transform, false);
                //n.transform.position = new Vector3(i * pieceWidth, j * pieceHeight, 0);
            }
        }
    }

	
	
	void Update () {
		
	}
}
