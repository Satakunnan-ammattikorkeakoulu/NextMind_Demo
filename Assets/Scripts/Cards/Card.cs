using System;
using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    private const float FlipDuration = 0.5f;

    private bool isFlipping;

    public bool isFlipped;
    public bool isMatched;
    public int id;
    public int pair;
    public event Action OnCardFlipped;

    public void Flip()
    {
        // Debug.Log($"Flip {id}, Pair: {pair}");
        if (MemoryGame.isWaiting) return;
        if (isFlipped)
        {
            StartCoroutine(FlipCard(transform.rotation, Quaternion.Euler(0, 0, 0)));
            isFlipped = false;
        }
        else
        {
            StartCoroutine(FlipCard(transform.rotation, Quaternion.Euler(0, 180, 0)));
            isFlipped = true;
            CardFlipped();
        }
    }

    // This will be replaced with the Flip() method for NextMind controller
    /*
    private void OnMouseDown()
    {
        // Debug.Log($"Card: {id}, Pair: {pair}");
        if (isFlipped)
        {
            StartCoroutine(FlipCard(transform.rotation, Quaternion.Euler(0, 0, 0)));
            isFlipped = false;
        }
        else
        {
            StartCoroutine(FlipCard(transform.rotation, Quaternion.Euler(0, 180, 0)));
            isFlipped = true;
            CardFlipped();
        }
    }
    */

    private void CardFlipped()
    {
        // Debug.Log("Card flip invoked!");
        OnCardFlipped?.Invoke();
    }

    private IEnumerator FlipCard(Quaternion startRotation, Quaternion endRotation)
    {
        var t = 0f;
        while (t < FlipDuration)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t / FlipDuration);
            yield return null;
        }
    }
}