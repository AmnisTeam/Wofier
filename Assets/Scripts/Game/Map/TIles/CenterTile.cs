using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTile : LetterTile
{
    public GameObject centerHexagon;

    public override void OnHaveLetter(bool isHaveLetter)
    {
        base.OnHaveLetter(isHaveLetter);
        if (isHaveLetter)
        {
            centerHexagon.SetActive(false);
        }
        else
        {
            centerHexagon.SetActive(true);
        }
    }

    public override int CanCompleteWord(TileWord word)
    {
        if (inventory.gamePlayManager.numberOfPlayerStep == 0)
            return 10000;
        return 1;
    }
}
