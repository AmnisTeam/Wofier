using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterItem : Item
{
    public TMPro.TMP_Text letterText;
    public TMPro.TMP_Text price;
    private char oldLetter;
    public char letter = 'A';

    public static float GetLetterPrice(char letter, float priceFactor)
    {
        switch (letter)
        {
            case 'A': return 1 * priceFactor;
            case 'B': return 3 * priceFactor;
            case 'C': return 3 * priceFactor;
            case 'D': return 2 * priceFactor;
            case 'E': return 1 * priceFactor;
            case 'F': return 4 * priceFactor;
            case 'G': return 2 * priceFactor;
            case 'H': return 4 * priceFactor;
            case 'I': return 1 * priceFactor;
            case 'J': return 7 * priceFactor;
            case 'K': return 5 * priceFactor;
            case 'L': return 4 * priceFactor;
            case 'M': return 3 * priceFactor;
            case 'N': return 1 * priceFactor;
            case 'O': return 1 * priceFactor;
            case 'P': return 3 * priceFactor;
            case 'Q': return 8 * priceFactor;
            case 'R': return 1 * priceFactor;
            case 'S': return 2 * priceFactor;
            case 'T': return 2 * priceFactor;
            case 'U': return 5 * priceFactor;
            case 'V': return 4 * priceFactor;
            case 'W': return 4 * priceFactor;
            case 'X': return 7 * priceFactor;
            case 'Y': return 2 * priceFactor;
            case 'Z': return 10 * priceFactor;
            default: return 0;
            //default: return 1 * priceFactor;
        }
    }

    private void Awake()
    {
        oldLetter = letter;
    }

    void Start()
    {
        price.text = GetLetterPrice(letter, 1).ToString();
    }

    void Update()
    {
        if (oldLetter != letter)
        {
            letterText.text = letter.ToString();
            oldLetter = letter;
        }
    }

    public override GameObject GetDraggedObject()
    {
        return null;
    }
}
