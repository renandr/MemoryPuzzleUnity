using System;
using System.Collections.Generic;
using UnityEngine;


public class Gameplay : MonoBehaviour {

    public Card CardTemplate;
    public Grid CardGrid;
    public List<Sprite> Faces;

    private Card firstCardOfPair;
    private Card secondCardOfPair;

    private static Gameplay singleton;

    void Awake() {
        singleton = this;
    }

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
	
	public static void CardClicked(Card card) {
        singleton.SelectCard(card);
    }
    private void SelectCard(Card card) {
        if (!firstCardOfPair) {
            firstCardOfPair = card;
            card.Select();

        } else if(card != firstCardOfPair && !secondCardOfPair) {
            secondCardOfPair = card;
            card.Select();
        }
    }
}
