using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public GamePlayManager gamePlayManager;
    public EndMenuManager endMenuManager;
    public CanvasGroup transitionScreen;
    public GameObject gameInterface;

    public Transform cameraPosition;
    public float cameraSize;

    public float timeToAppereanceTransitionScreen;

    public WinnerPerson GetTopWinner()
    {
        int maxPoints = 0;
        TileWord maxTileWord = null;
        for(int x = 0; x < gamePlayManager.words.Count; x++)
        {
            int points = 0;
            TileWord tileWord = gamePlayManager.words[x];
            for(int y = 0; y < tileWord.tiles.Count; y++)
            {
                LetterTile letterTile = gamePlayManager.mapGenerator.map[tileWord.tiles[y].x][tileWord.tiles[y].y].GetComponent<LetterTile>();
                points += (int)letterTile.GetLetterPrice();
            }

            if(points > maxPoints)
            {
                maxPoints = points;
                maxTileWord = tileWord;
            }
        }

        TilesWordInfo word = new TilesWordInfo();
        
        for (int x = 0; x < maxTileWord.tiles.Count; x++)
        {
            int id = Mathf.Abs(maxTileWord.tiles[1].x - maxTileWord.tiles[0].x) > 0 ? x : maxTileWord.tiles.Count - 1 - x;
            LetterTile letterTile = gamePlayManager.mapGenerator.map[maxTileWord.tiles[id].x][maxTileWord.tiles[id].y].GetComponent<LetterTile>();
            word.tiles.Add(new LetterWithPoints(letterTile.letter, (int)letterTile.GetLetterPrice()));
        }

        WinnerPerson winnerPerson = new WinnerPerson();
        winnerPerson.person = maxTileWord.person;
        winnerPerson.word = word;

        return winnerPerson;
    }

    public WinnerPerson GetLongestWinner()
    {
        int maxCountLetters = 0;
        TileWord maxTileWord = null;
        for (int x = 0; x < gamePlayManager.words.Count; x++)
        {
            TileWord tileWord = gamePlayManager.words[x];

            if (tileWord.tiles.Count > maxCountLetters)
            {
                maxCountLetters = tileWord.tiles.Count;
                maxTileWord = tileWord;
            }
        }

        TilesWordInfo word = new TilesWordInfo();
        for (int x = 0; x < maxTileWord.tiles.Count; x++)
        {
            int id = Mathf.Abs(maxTileWord.tiles[1].x - maxTileWord.tiles[0].x) > 0 ? x : maxTileWord.tiles.Count - 1 - x;
            LetterTile letterTile = gamePlayManager.mapGenerator.map[maxTileWord.tiles[id].x][maxTileWord.tiles[id].y].GetComponent<LetterTile>();
            word.tiles.Add(new LetterWithPoints(letterTile.letter, (int)letterTile.GetLetterPrice()));
        }

        WinnerPerson winnerPerson = new WinnerPerson();
        winnerPerson.person = maxTileWord.person;
        winnerPerson.word = word;

        return winnerPerson;
    }

    public void OpenEndGameMenu()
    {
        transitionScreen.gameObject.SetActive(true);
        transitionScreen.LeanAlpha(1, timeToAppereanceTransitionScreen).setOnComplete(() => { 
            endMenuManager.gameObject.SetActive(true);
            gameInterface.SetActive(false);
            Camera.main.transform.position = cameraPosition.position;
            Camera.main.GetComponent<MoveOnMapCamera>().isLocked = true;
            Camera.main.orthographicSize = cameraSize;


            List<Person> persons = new List<Person>();
            for (int x = 0; x < gamePlayManager.personManager.persons.Count; x++)
                persons.Add(gamePlayManager.personManager.persons[x]);

            endMenuManager.SetEndMenuData(persons, GetTopWinner(), GetLongestWinner());

        });
        transitionScreen.LeanAlpha(0, timeToAppereanceTransitionScreen)
                        .setDelay(timeToAppereanceTransitionScreen)
                        .setOnComplete(() => { transitionScreen.gameObject.SetActive(false); });
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
