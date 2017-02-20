using System;
using System.Collections.Generic;
using UnityEngine;


public class Gameplay : MonoBehaviour {

    public Card CardTemplate;
    public Grid CardGrid;
    public List<Sprite> Faces;
    

	void Start () {
        var cards = new List<GameObject>();
        for (int iFace = 0; iFace < Faces.Count; iFace++) {
            Card c = Instantiate(CardTemplate);
            c.SetFace(Faces[iFace], iFace);
            cards.Add(c.gameObject);

            var cPair = Instantiate(CardTemplate);
            cPair.SetFace(Faces[iFace], iFace);
            cards.Add(cPair.gameObject);
        }

        new System.Random().Shuffle(cards);
        CardGrid.AddItems(cards);
    }
	
	void Update () {
		
	}
}
