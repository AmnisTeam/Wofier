using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterTile : Tile
{
    public TMPro.TMP_Text letterText;
    public char letter;
    private char oldLetter;

    public bool isHaveLetter = false;
    private bool isHaveLetterOld = false;

    public Color colorWithLetter;
    private Color oldColor;

    public void SetLetter(char letter)
    {
        this.letter = letter;
        isHaveLetter = true;
    }

    public void UnsetLetter()
    {
        isHaveLetter = false;
    }

    public override void OnSetItem(Item item)
    {
        LetterItem letterItem = item as LetterItem;
        if(letterItem)
        {
            letter = letterItem.letter;
            isHaveLetter = true;
        }
    }

    void Awake()
    {
        oldLetter = letter;
        isHaveLetterOld = isHaveLetter;
    }

    void Start()
    {

    }


    void Update()
    {
        if (oldLetter != letter)
        {
            letterText.text = letter.ToString();
            oldLetter = letter;
        }

        if (isHaveLetterOld != isHaveLetter)
        {
            isHaveLetterOld = isHaveLetter;
            if (isHaveLetter)
            {
                letterText.gameObject.SetActive(true);
                oldColor = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = colorWithLetter;
            }
            else
            {
                letterText.gameObject.SetActive(false);
                GetComponent<SpriteRenderer>().color = oldColor;
            }
        }
    }
}
