using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TileWord
{
    public Vector2Int[] tiles;
}

public class GamePlayManager : MonoBehaviour
{
    public Person me;
    public PersonsManager personManager;
    public ScoreTableManager scoreTableManager;
    public MapGenerator mapGenerator;

    public WordDictionary wordDictionary;

    public GameObject findingMenu;
    public TMPro.TMP_Text findingMenuNickname;

    public TMPro.TMP_Text timeNickname;
    public TMPro.TMP_Text time;

    public int idPlayingPerson = -1;

    public float timeToPlayingOnePerson;
    private float timerToPlayerOnePerson = float.NaN;

    public float timeToAppearanceFindingMenu;
    public float findingMenuShowingTime;
    private float findingMenuShowingTimer = float.NaN;

    public void SelectNextPersonToPlay()
    {
        idPlayingPerson = (idPlayingPerson + 1) % personManager.persons.Count;
        timerToPlayerOnePerson = timeToPlayingOnePerson;

        findingMenuNickname.text = personManager.persons[idPlayingPerson].nickname;
        findingMenuNickname.color = personManager.persons[idPlayingPerson].color;
        findingMenu.SetActive(true);
        findingMenu.GetComponent<CanvasGroup>().LeanAlpha(1, timeToAppearanceFindingMenu);
        findingMenuShowingTimer = findingMenuShowingTime;

        timeNickname.text = personManager.persons[idPlayingPerson].nickname;
        timeNickname.color = personManager.persons[idPlayingPerson].color;

        scoreTableManager.updateTable();
    }

    public void OnCompleteAnitaion(object gameObject)
    {
        (gameObject as GameObject).SetActive(false);
    }

    public bool IsTileRaw(LetterTile tile)
    {
        return tile != null && tile.isHaveLetter && tile.person.id == personManager.persons[idPlayingPerson].id;
    }

    public void FindWords()
    {
        List<TileWord> words = new List<TileWord>();
        List<TileWord> rawWords = new List<TileWord>();

        bool isRawWord = false;

        for(int x = 0; x < mapGenerator.mapSizeX; x++)
            for(int y = 0; y < mapGenerator.mapSizeY; y++)
            {
                LetterTile tile = mapGenerator.map[x][y].GetComponent<LetterTile>();

                if(IsTileRaw(tile)) //Значит что сырая клетка
                {
                    string horizontalWord = "";
                    string verticalWord = "";

                    int pointerX = x - 1;
                    while (pointerX >= 0 && IsTileRaw(mapGenerator.map[pointerX][y].GetComponent<LetterTile>()))
                        pointerX--;
                    pointerX++;
              
                    while (pointerX < mapGenerator.mapSizeX && IsTileRaw(mapGenerator.map[pointerX][y].GetComponent<LetterTile>()))
                    {
                        horizontalWord += mapGenerator.map[pointerX][y].GetComponent<LetterTile>().letter;
                        pointerX++;
                    }

                    int pointerY = y - 1;
                    while (pointerY >= 0 && IsTileRaw(mapGenerator.map[x][pointerY].GetComponent<LetterTile>()))
                        pointerY--;
                    pointerY++;

                    while (pointerY < mapGenerator.mapSizeY && IsTileRaw(mapGenerator.map[x][pointerY].GetComponent<LetterTile>()))
                    {
                        verticalWord += mapGenerator.map[x][pointerY].GetComponent<LetterTile>().letter;
                        pointerY++;
                    }

                    wordDictionary.checkWord(horizontalWord);
                }

            }




        int pointStart = 0, pointEnd = 0;
        for(int y = 0; y < mapGenerator.mapSizeY; y++)
            for(int x = 1; x < mapGenerator.mapSizeX + 1; x++)
            {
                Tile tilePrevious = mapGenerator.map[x - 1][y].GetComponent<Tile>();
                Tile tileCurrent = x < mapGenerator.mapSizeX ? mapGenerator.map[x][y].GetComponent<Tile>() : null;

                bool isWordPartPervious = (tilePrevious is LetterTile) && (tilePrevious as LetterTile).isHaveLetter;
                bool isWordPartCurrent = tileCurrent ? (tileCurrent is LetterTile) && (tileCurrent as LetterTile).isHaveLetter : false;

                LetterTile tileCurrentLetter = tileCurrent as LetterTile;
                if (tileCurrent != null && 
                    tileCurrentLetter.isHaveLetter && 
                    tileCurrentLetter.person != null && 
                    tileCurrentLetter.person.id == personManager.persons[idPlayingPerson].id)
                    isRawWord = true;

                if (!isWordPartPervious && isWordPartCurrent) // Начало слова
                {
                    pointStart = x;
                }

                if(isWordPartPervious && !isWordPartCurrent) // Конец слова
                {
                    pointEnd = x - 1;

                    
                }
            }
    }

    void Awake()
    {
        personManager.connectPerson(me);
        personManager.connectPerson(new Person(1, "ThEnd", new Color(1, 0.6f, 0.6f, 1), 1));
        personManager.connectPerson(new Person(2, "DotaKot", new Color(0.6f, 1, 0.6f, 1), 2));
        personManager.connectPerson(new Person(3, "SpectreSpect", new Color(0.6f, 0.6f, 1, 1), 3));
        personManager.connectPerson(new Person(4, "Hexumee", new Color(1, 0.6f, 1, 1), 4));
        personManager.persons[1].score = 200;

        wordDictionary = new TempDictionary();
    }

    void Start()
    {
        SelectNextPersonToPlay();
    }

    void Update()
    {
        findingMenuShowingTimer -= Time.deltaTime;
        if(findingMenuShowingTimer < 0)
        {
            findingMenu.GetComponent<CanvasGroup>().LeanAlpha(0, timeToAppearanceFindingMenu).setOnComplete(OnCompleteAnitaion, findingMenu);
            findingMenuShowingTimer = float.NaN;
        }

        timerToPlayerOnePerson -= Time.deltaTime;
        if(timerToPlayerOnePerson < 0)
        {
            SelectNextPersonToPlay();
        }

        int secundes = (int)(timerToPlayerOnePerson % 60);
        int minutes = (int)(timerToPlayerOnePerson / 60);

        time.text = minutes.ToString("00") + ":" + secundes.ToString("00");
    }
}
