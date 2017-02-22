using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the gameplay, the rules of the game. Avoid handling things too specific here.
/// </summary>
public class Gameplay : MonoBehaviour {

    [Header("Elements")]
    public Card CardTemplate;
    public Grid CardGrid;
    public TopControls TopControls;

    [Header("Gameplay configuration")]
    [Tooltip("Time in seconds to start showing the cards when the game starts.")]
    public float ShowAllCardsDelay;

    [Tooltip("Time in seconds that the player has to memorize the cards.")]
    public float ShowAllCardsTime;

    [Tooltip("Time in seconds to display the failed pair.")]
    public float ShowPairFailTime;

    [Tooltip("Time in seconds for the game.")]
    public float FullGameTime;

    [Tooltip("Sprites for the faces that are used by this game.")]
    public List<Sprite> Faces;

    /// <summary>
    /// Used privately for providing access from other classes to specific static methods
    /// </summary>
    private static Gameplay singleton;

    /// <summary>
    /// Lists all cards in the game
    /// </summary>
    private List<Card> allCards;

    /// <summary>
    /// First card selected by the player when attempting to find a match
    /// </summary>
    private Card firstCardOfPair;

    /// <summary>
    /// Second card selected by the player, when this one is assigned the game checks if it's a match
    /// </summary>
    private Card secondCardOfPair;

    /// <summary>
    /// Pairs remaining to finish the game, initial value depends on the amount of the faces
    /// </summary>
    private int pairsRemaining;

    /// <summary>
    /// Current game time
    /// </summary>
    private float currentTime;

    /// <summary>
    ///Used for pausing and between games
    /// </summary>
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
        // resets everything
        StopAllCoroutines();
        firstCardOfPair = secondCardOfPair = null;
        TopControls.EnableControls(false);
        TopControls.TogglePlay(false);
        CardGrid.SetAlpha(1f);

        //resets the counters
        pairsRemaining = Faces.Count;
        currentTime = FullGameTime;

        //adds the reshuffled card game objects to the grid
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

        //player is allowed to play
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
        StopAllCoroutines();

        for (int i = 0; i < allCards.Count; i++) {
            allCards[i].Show();
        }
        
        TopControls.TogglePlay(true);
        CardGrid.SetAlpha(0.5f);
        pairsRemaining = 0;
    }

    private void PlayerWon() {
        TopControls.ShowWin();
        gameRunning = false;
        StopAllCoroutines();
        TopControls.TogglePlay(true);
    }
}
