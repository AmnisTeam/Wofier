using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTile : LetterTile
{
    public GameObject powerText;

    public override void OnHaveLetter(bool isHaveLetter)
    {
        base.OnHaveLetter(isHaveLetter);

        if(isHaveLetter)
        {
            powerText.SetActive(false);
        }
        else
        {
            powerText.SetActive(true);
        }
    }
}
