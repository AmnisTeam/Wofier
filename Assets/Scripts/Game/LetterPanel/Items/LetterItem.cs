using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterItem : Item
{
    public TMPro.TMP_Text letterText;
    private char oldLetter;
    public char letter = 'A';

    private void Awake()
    {
        oldLetter = letter;
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
    }

    public override GameObject GetDraggedObject()
    {
        return null;
    }
}
