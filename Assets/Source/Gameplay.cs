using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gameplay : MonoBehaviour {

    public Card CardTemplate;
    public Grid CardGrid;
    public TopControls TopControls;
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
    private float currentTime;
    private bool gameRunning;
    
    void Awake() {
        singleton = this;
    }

	void Start () {
        CreateAllCards();
        TopControls.TogglePlay(true);
        TopControls.EnableControls(true);
        TopControls.Time = FullGameTime;
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

    public void UI_PauseClicked() {
        CardGrid.SetAlpha(0.2f);
        TopControls.TogglePlay(true);
        gameRunning = false;
    }

    public void UI_ReplayClicked() {
        StartGame();
    }

    public void UI_PlayClicked() {
        if (pairsRemaining == 0) {
            StartGame();
        } else {
            CardGrid.SetAlpha(1f);
            TopControls.TogglePlay(false);
            gameRunning = true;
        }
    }

    private void StartGame() {
        StopAllCoroutines();
        firstCardOfPair = secondCardOfPair = null;
        TopControls.EnableControls(false);
        TopControls.TogglePlay(false);
        CardGrid.SetAlpha(1f);

        pairsRemaining = Faces.Count;
        currentTime = FullGameTime;

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
        TopControls.EnableControls(true);
        gameRunning = true;
    }

    private void Update() {
       if(gameRunning) {
            currentTime -= Time.deltaTime;
            TopControls.Time = currentTime;
            if(currentTime <= 0) {
                PlayerLost();
            }
       } 
    }

    public static void CardClicked(Card card) {
        singleton.SelectCard(card);
    }

    private void SelectCard(Card card) {
        if (!gameRunning || !card.IsFacingBack) {
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
                    PlayerWon();
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

    private void PlayerLost() {
        TopControls.ShowLose();
        gameRunning = false;

        for (int i = 0; i < allCards.Count; i++) {
            allCards[i].Show();
        }
        
        TopControls.TogglePlay(true);
        CardGrid.SetAlpha(0.5f);
    }

    private void PlayerWon() {
        TopControls.ShowWin();
        gameRunning = false;
        TopControls.TogglePlay(true);
    }
}
