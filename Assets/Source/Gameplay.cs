using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gameplay : MonoBehaviour {

    public Card CardTemplate;
    public Grid CardGrid;
    public List<Sprite> Faces;
    public float DisplayCardsTimeSeconds;

    private static Gameplay singleton;

    private Card firstCardOfPair;
    private Card secondCardOfPair;
    private int pairsRemaining;
    private Coroutine waitforHideCoroutine;

    void Awake() {
        singleton = this;
    }

	void Start () {
        var cards = new List<GameObject>();
        pairsRemaining = Faces.Count;
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
            card.Show();

        } else if (card != firstCardOfPair && !secondCardOfPair) {
            secondCardOfPair = card;
            card.Show();

            if (firstCardOfPair.Index == secondCardOfPair.Index) {
                firstCardOfPair.Win();
                secondCardOfPair.Win();
                firstCardOfPair = secondCardOfPair = null;
                pairsRemaining--;
                if (pairsRemaining == 0) {
                    Debug.Log("GameOver");
                }
            } else {
                waitforHideCoroutine = StartCoroutine(WaitAndHide(DisplayCardsTimeSeconds));
            }
        }
    }
           

    private IEnumerator WaitAndHide(float waitTime) {
        while (true) {
            yield return new WaitForSeconds(waitTime);
            firstCardOfPair.Hide();
            secondCardOfPair.Hide();
            firstCardOfPair = secondCardOfPair = null;
            StopCoroutine(waitforHideCoroutine);
        }
    }

}
