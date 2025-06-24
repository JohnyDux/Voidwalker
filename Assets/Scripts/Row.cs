using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public Sprite[] symbols; // Array of symbols for the row
    public SpriteRenderer[] symbolRenderers; // Renderers for displaying symbols
    public float spinSpeed = 10f; // Speed of the spin

    private Sprite[] currentSymbols = new Sprite[3]; // Stores current, previous, and older symbols

    int randomIndex;
    public int winSpriteIndex;

    public void Spin()
    {
        StartCoroutine(SpinCoroutine());
    }

    private IEnumerator SpinCoroutine()
    {
        float timeElapsed = 0f;

        while (timeElapsed < spinSpeed)
        {
            // Rotate or change symbols here
            for (int i = 0; i < symbolRenderers.Length; i++)
            {
                randomIndex = Random.Range(0, symbols.Length);
                SetNewSymbol(symbols[randomIndex]);
            }

            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        winSpriteIndex = randomIndex;

        // Optionally, you can stop at a specific symbol or implement easing
    }

    public void SetNewSymbol(Sprite newSymbol)
    {
        // Shift symbols back (current becomes previous, previous becomes older)
        currentSymbols[2] = currentSymbols[1];
        currentSymbols[1] = currentSymbols[0];
        currentSymbols[0] = newSymbol;

        // Update all sprite renderers
        for (int i = 0; i < symbolRenderers.Length; i++)
        {
            symbolRenderers[i].sprite = currentSymbols[i];
        }
    }
}