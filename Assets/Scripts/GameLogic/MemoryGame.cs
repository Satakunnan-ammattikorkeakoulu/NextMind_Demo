using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NextMind.Examples;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class MemoryGame : MonoBehaviour
{
    private const float FlipBackDelay = 1.5f;
    private int fails;
    private int matches;

    public AudioSource correct;
    public AudioSource wrong;
    public GameObject endGameScreen;
    public TMP_Text endText;
    public List<GameObject> cards;
    public int flippedCards;

    public static bool isWaiting;

    private void Start()
    {
        flippedCards = 0;
        fails = 0;
        matches = 0;

        ShuffleCards();

        cards.ForEach(c => c.GetComponent<Card>().OnCardFlipped += CardFlipped);
    }

    private void Update()
    {
        if (matches == 4) YouWin();
        if (fails == 3) YouLose();
    }

    private void CardFlipped()
    {
        flippedCards++;
        if (flippedCards == 2)
        {
            flippedCards = 0;
            StartCoroutine(CheckCards());
        }
    }

    private IEnumerator CheckCards()
    {
        isWaiting = true;
        yield return new WaitForSeconds(FlipBackDelay);
        isWaiting = false;
        var cardList = cards.Where(c => c.GetComponent<Card>().isFlipped && !c.GetComponent<Card>().isMatched)
            .ToList();
        if (cardList.Count == 2)
        {
            var card1 = cardList[0].GetComponent<Card>();
            var card2 = cardList[1].GetComponent<Card>();
            if (card1.pair == card2.pair)
            {
                // Debug.Log("The two cards match!");
                correct.Play();
                matches++;
                card1.isMatched = true;
                card2.isMatched = true;
            }
            else
            {
                // Debug.Log("The two cards don't match!");
                wrong.Play();
                fails++;
                card1.Flip();
                card2.Flip();
            }
        }
    }

    private void ShuffleCards()
    {
        var rand = new Random();
        var grid = Enumerable.Repeat(Vector3.zero, 8).ToList();
        var shuffledCards = cards.OrderBy(c => rand.Next()).ToList();

        for (var i = 0; i < grid.Count; i++)
            if (grid[i] == Vector3.zero)
            {
                grid[i] = shuffledCards[0].transform.position;
                shuffledCards.RemoveAt(0);
            }

        for (var i = 0; i < cards.Count; i++) cards[i].transform.position = grid[i];
    }

    private void YouWin()
    {
        DeactivateCards();
        endText.text = "You Win!";
        endGameScreen.SetActive(true);
    }

    private void YouLose()
    {
        DeactivateCards();
        endText.text = "You Lose";
        endGameScreen.SetActive(true);
    }

    private void DeactivateCards()
    {
        cards.ForEach(c => c.SetActive(false));
    }
}