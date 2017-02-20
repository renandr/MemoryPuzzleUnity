using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour {

    public Card CardTemplate;
    public Grid CardGrid;
    public List<Sprite> Faces;
    

	void Start () {
        for (int iFace = 0; iFace < Faces.Count; iFace++) {
            Card c = Instantiate(CardTemplate);
            c.SetFace(Faces[iFace], iFace);
            CardGrid.AddItem(c.gameObject);

            var cPair = Instantiate(CardTemplate);
            cPair.SetFace(Faces[iFace], iFace);
            CardGrid.AddItem(cPair.gameObject);
        }
    }
	
	void Update () {
		
	}
}
