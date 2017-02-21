using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gameplay : MonoBehaviour {

    public Card CardTemplate;
    public Grid CardGrid;
    
    public float ShowAllCardsDelay;
    public float ShowAllCardsTime;
    public float ShowPairFailTime;

    public List<Sprite> Faces;
    

    private static Gameplay singleton;

    private Card firstCardOfPair;
    private Card secondCardOfPair;
    private int pairsRemaining;

    private List<Card> allCards;

    void Awake() {
        singleton = this;
    }

	void Start () {
        pairsRemaining = Faces.Count;
        CreateAllCards();
        StartCoroutine(WaitAndShowAll());
    }
    private void CreateAllCards() {
        allCards = new List<Card>();
        var cardGOsForGrid = new List<GameObject>();

        for (int iFace = 0; iFace < Faces.Count; iFace++) {
            Card card = CreateCard(iFace);
            cardGOsForGrid.Add(card.gameObject);
            allCards.Add(card);

            Card pair = CreateCard(iFace);
            cardGOsForGrid.Add(pair.gameObject);
            allCards.Add(pair);
        }

        new System.Random().Shuffle(cardGOsForGrid);
        CardGrid.AddItems(cardGOsForGrid);
    }

    private Card CreateCard(int faceIndex) {
        Card c = Instantiate(CardTemplate);
        c.SetFace(Faces[faceIndex], faceIndex);
        return c;
    }

    private IEnumerator WaitAndShowAll() {
        yield return new WaitForSeconds(ShowAllCardsDelay);
        for (int i = 0; i < allCards.Count; i++) {
            allCards[i].Show();
        }
            
        yield return new WaitForSeconds(ShowAllCardsTime);
        for (int i = 0; i < allCards.Count; i++) {
            allCards[i].Hide();
        }
    }
    
    public static void CardClicked(Card card) {
        singleton.SelectCard(card);
    }

    private void SelectCard(Card card) {
        if (!card.IsFacingBack) {
            return;
        }
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
                StartCoroutine(WaitAndHidePairFail());
            }
        }
    }
           

    private IEnumerator WaitAndHidePairFail() { 
        yield return new WaitForSeconds(ShowPairFailTime);
        firstCardOfPair.Hide();
        secondCardOfPair.Hide();
        firstCardOfPair = secondCardOfPair = null;
    }

}
