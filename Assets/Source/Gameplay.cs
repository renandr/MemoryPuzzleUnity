using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gameplay : MonoBehaviour {

    public Card CardTemplate;
    public Grid CardGrid;
    public TopButtons TopButtons;
    public float ShowAllCardsDelay;
    public float ShowAllCardsTime;
    public float ShowPairFailTime;
    public float FullGameTime;
    public List<Sprite> Faces;
    
    private static Gameplay singleton;

    private List<Card> allCards;
    private Card firstCardOfPair;
    private Card secondCardOfPair;
    private int pairsRemaining;

    
    void Awake() {
        singleton = this;
    }

	void Start () {
        CreateAllCards();
        TopButtons.TogglePlay(true);
        TopButtons.EnableControls(true);
    }

    private void CreateAllCards() {
        allCards = new List<Card>();
        for (int iFace = 0; iFace < Faces.Count; iFace++) {
            CreateCard(iFace);
            CreateCard(iFace);
        }
    }

    private Card CreateCard(int faceIndex) {
        Card c = Instantiate(CardTemplate);
        c.SetFace(Faces[faceIndex], faceIndex);
        allCards.Add(c);
        return c;
    }

    public void StartGame() {
        StopAllCoroutines();
        firstCardOfPair = secondCardOfPair = null;
        TopButtons.EnableControls(false);
        TopButtons.TogglePlay(false);

        pairsRemaining = Faces.Count;
        
        var cardGOsForGrid = new List<GameObject>();
        for (int i = 0; i < allCards.Count; i++) {
            Card card = allCards[i];
            card.Hide(false);
            cardGOsForGrid.Add(card.gameObject);
        }
        new System.Random().Shuffle(cardGOsForGrid);
        CardGrid.AddItems(cardGOsForGrid);

        StartCoroutine(WaitAndShowAll());
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
        TopButtons.EnableControls(true);
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

    public void PauseGame() {

    }

    public void RestartGame() {
        StartGame();
    }
}
