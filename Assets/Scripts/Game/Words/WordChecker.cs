using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordChecker
{
    float addedScore;
    public GamePlayManager gamePlayManager;

    public WordChecker(GamePlayManager gamePlayManager)
    {
        this.gamePlayManager = gamePlayManager;
    }

    public bool IsTileRaw(LetterTile tile)
    {
        return tile != null && tile.isHaveLetter && !tile.inWord && tile.person.id == gamePlayManager.personManager.persons[gamePlayManager.idPlayingPerson].id;
    }

    public bool IsLetterTileWithLetter(LetterTile tile)
    {
        return tile != null && tile.isHaveLetter;
    }

    public List<TileWord> CheckWords(out float addedScores)
    {
        List<TileWord> words = new List<TileWord>();
        Dictionary<string, bool> isCheckWord = new Dictionary<string, bool>();

        float countScore = 0;

        for (int x = 0; x < gamePlayManager.mapGenerator.mapSizeX; x++)
            for (int y = 0; y < gamePlayManager.mapGenerator.mapSizeY; y++)
            {
                LetterTile tile = gamePlayManager.mapGenerator.map[x][y].GetComponent<LetterTile>();

                if (IsTileRaw(tile)) //«начит что сыра€ клетка
                {
                    string horizontalWord = "";
                    string verticalWord = "";

                    string keyHorizontalWord = "";
                    string keyVerticalWord = "";

                    float horizontalScore = 0;
                    float verticalScore = 0;

                    bool isInWordHorizontal = false;
                    bool isInWordVertical = false;

                    TileWord horizonralTileWord = new TileWord();
                    TileWord verticalTileWord = new TileWord();

                    horizonralTileWord.person = gamePlayManager.me;
                    verticalTileWord.person = gamePlayManager.me;

                    int pointerX = x - 1;
                    while (pointerX >= 0 && IsLetterTileWithLetter(gamePlayManager.mapGenerator.map[pointerX][y].GetComponent<LetterTile>()))
                        pointerX--;
                    pointerX++;

                    while (pointerX < gamePlayManager.mapGenerator.mapSizeX && IsLetterTileWithLetter(gamePlayManager.mapGenerator.map[pointerX][y].GetComponent<LetterTile>()))
                    {
                        horizontalWord += gamePlayManager.mapGenerator.map[pointerX][y].GetComponent<LetterTile>().letter;
                        horizontalScore += gamePlayManager.mapGenerator.map[pointerX][y].GetComponent<LetterTile>().GetLetterPrice();
                        if (gamePlayManager.mapGenerator.map[pointerX][y].GetComponent<LetterTile>().inWord)
                            isInWordHorizontal = true;
                        keyHorizontalWord += pointerX.ToString() + ";" + y.ToString();
                        horizonralTileWord.tiles.Add(new Vector2Int(pointerX, y));
                        pointerX++;
                    }

                    int pointerY = y - 1;
                    while (pointerY >= 0 && IsLetterTileWithLetter(gamePlayManager.mapGenerator.map[x][pointerY].GetComponent<LetterTile>()))
                        pointerY--;
                    pointerY++;

                    while (pointerY < gamePlayManager.mapGenerator.mapSizeY && IsLetterTileWithLetter(gamePlayManager.mapGenerator.map[x][pointerY].GetComponent<LetterTile>()))
                    {
                        verticalWord += gamePlayManager.mapGenerator.map[x][pointerY].GetComponent<LetterTile>().letter;
                        verticalScore += gamePlayManager.mapGenerator.map[x][pointerY].GetComponent<LetterTile>().GetLetterPrice();
                        if (gamePlayManager.mapGenerator.map[x][pointerY].GetComponent<LetterTile>().inWord)
                            isInWordVertical = true;
                        keyVerticalWord += x.ToString() + ";" + pointerY.ToString();
                        verticalTileWord.tiles.Add(new Vector2Int(x, pointerY));
                        pointerY++;
                    }

                    char[] reverseWord = verticalWord.ToCharArray();
                    Array.Reverse(reverseWord);
                    verticalWord = new string(reverseWord);

                    if (!isCheckWord.ContainsKey(keyHorizontalWord) && horizontalWord.Length > 1 && gamePlayManager.wordDictionary.checkWord(horizontalWord))
                    {
                        countScore += horizontalScore;
                        isCheckWord.Add(keyHorizontalWord, true);
                        words.Add(horizonralTileWord);
                    }

                    if (!isCheckWord.ContainsKey(keyVerticalWord) && verticalWord.Length > 1 && gamePlayManager.wordDictionary.checkWord(verticalWord))
                    {
                        countScore += verticalScore;
                        isCheckWord.Add(keyVerticalWord, true);
                        words.Add(verticalTileWord);
                    }

                    //≈сли ставитс€ первое слово
                    bool isFistWord = gamePlayManager.numberOfPlayerStep == 0 && gamePlayManager.personManager.persons[gamePlayManager.idPlayingPerson].id == gamePlayManager.me.id;
                    //“олько горизонтальное слово (вертикальное = 1)
                    bool haveHorizontalWord = verticalWord.Length == 1 && horizontalWord.Length > 1 && gamePlayManager.wordDictionary.checkWord(horizontalWord) && (isInWordHorizontal || isFistWord);
                    //“олько вертикальное слово (горизонтальное = 1)
                    bool haveVerticalWord = horizontalWord.Length == 1 && verticalWord.Length > 1 && gamePlayManager.wordDictionary.checkWord(verticalWord) && (isInWordVertical || isFistWord);
                    //» вертикальное слово, и горизонтальное слово
                    bool haveVerticalHorizontalWord = horizontalWord.Length > 1 && verticalWord.Length > 1 && gamePlayManager.wordDictionary.checkWord(horizontalWord) && gamePlayManager.wordDictionary.checkWord(verticalWord) && (isInWordHorizontal && isInWordVertical || isFistWord);

                    bool toPass = haveHorizontalWord || haveVerticalWord || haveVerticalHorizontalWord;

                    if (!toPass)
                    {
                        addedScores = 0;
                        return null;
                    }
                }
            }

        addedScores = countScore;
        return words;
    }

    public void TryFindWord()
    {
        if (gamePlayManager.personManager.persons[gamePlayManager.idPlayingPerson].id == PhotonNetwork.LocalPlayer.ActorNumber - 1)
        //if(personManager.persons[idPlayingPerson].id == me.id)
        {
            List<TileWord> words = CheckWords(out addedScore);
            bool wordIsFind = words != null;
            gamePlayManager.wordIsFind = wordIsFind;
            gamePlayManager.findWords = words;
            gamePlayManager.addedScore = addedScore;

            if (wordIsFind)
            {
                if (addedScore == 1)
                    gamePlayManager.scoresText.text = "(" + addedScore.ToString() + " очко)";
                else if (addedScore >= 2 && addedScore <= 4)
                    gamePlayManager.scoresText.text = "(" + addedScore.ToString() + " очка)";
                else
                    gamePlayManager.scoresText.text = "(" + addedScore.ToString() + " очков)";

                gamePlayManager.acceptWordButton.SetActive(true);
                gamePlayManager.acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(1, gamePlayManager.timeToAppearanceAcceptWordButton);
            }
            else
            {
                gamePlayManager.acceptWordButton.SetActive(false);
                gamePlayManager.acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(0, gamePlayManager.timeToAppearanceAcceptWordButton);
            }
            if (addedScore == 0)
            {
                gamePlayManager.acceptWordButton.SetActive(false);
                gamePlayManager.acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(0, gamePlayManager.timeToAppearanceAcceptWordButton);
            }
        }
    }
}
