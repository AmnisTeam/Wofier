using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerTile : LetterTile
{
    public GameObject powerText;

    public PowerTile()
    {

    }

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

    public override void CompleteWord(TileWord word)
    {
        base.CompleteWord(word);

        for(int x = 0; x < word.tiles.Count; x++)
        {
            LetterTile tile = inventory.gamePlayManager.mapGenerator.map[word.tiles[x].x][word.tiles[x].y].GetComponent<LetterTile>();
            if(tile)
            {
                tile.colorInWord = colorInWord;
                tile.letterText.color = letterText.color;
                tile.completeWordOrder = -1;
                tile.GetComponent<SpriteRenderer>().color = colorInWord;
            }
        }
    }
}
