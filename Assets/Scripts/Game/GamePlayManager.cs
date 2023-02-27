using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileWord
{
    public List<Vector2Int> tiles;

    public TileWord()
    {
        tiles = new List<Vector2Int>();
    }
}

public class GamePlayManager : MonoBehaviour
{
    //public Person me;
    public PersonsManager personManager;
    public ScoreTableManager scoreTableManager;
    public MapGenerator mapGenerator;

    public WordDictionary wordDictionary;

    public GameObject findingMenu;
    public TMPro.TMP_Text findingMenuNickname;

    public TMPro.TMP_Text timeNickname;
    public TMPro.TMP_Text time;

    public GameObject acceptWordButton;
    public GameObject acceptText;
    public TMPro.TMP_Text scoresText;

    public bool wordIsFind = false;
    public int addedScore = 0;

    public int idPlayingPerson = -1;

    public float timeToPlayingOnePerson;
    private float timerToPlayerOnePerson = float.NaN;

    public float timeToAppearanceFindingMenu;
    public float findingMenuShowingTime;
    private float findingMenuShowingTimer = float.NaN;

    public float timeToAppearanceAcceptWordButton;

    public string avatarSpritesTag;
    GameObject avatarSprites;
    IconsContent iconsContent;
    //Sprite[] icons;

    public string colorsHolderTag;
    private GameObject colorsHolder;
    private ColorsHolder instanceColorHolder;

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

    public string VectorsToString(Vector2Int[] vectors)
    {
        string str = "";
        for(int x = 0; x < vectors.Length; x++)
            str += vectors[x].x.ToString() + ";" + vectors[x].y.ToString();
        return str;
    }

    public List<TileWord> CheckWords(out int addedScores)
    {
        List<TileWord> words = new List<TileWord>();
        Dictionary<string, bool> isCheckWord = new Dictionary<string, bool>();

        int countScore = 0;

        for(int x = 0; x < mapGenerator.mapSizeX; x++)
            for(int y = 0; y < mapGenerator.mapSizeY; y++)
            {
                LetterTile tile = mapGenerator.map[x][y].GetComponent<LetterTile>();

                if(IsTileRaw(tile)) //Значит что сырая клетка
                {
                    string horizontalWord = "";
                    string verticalWord = "";

                    string keyHorizontalWord = "";
                    string keyVerticalWord = "";

                    int horizontalScore = 0;
                    int verticalScore = 0;

                    TileWord horizonralTileWord = new TileWord();
                    TileWord verticalTileWord = new TileWord();

                    int pointerX = x - 1;
                    while (pointerX >= 0 && IsTileRaw(mapGenerator.map[pointerX][y].GetComponent<LetterTile>()))
                        pointerX--;
                    pointerX++;
              
                    while (pointerX < mapGenerator.mapSizeX && IsTileRaw(mapGenerator.map[pointerX][y].GetComponent<LetterTile>()))
                    {
                        horizontalWord += mapGenerator.map[pointerX][y].GetComponent<LetterTile>().letter;
                        horizontalScore += mapGenerator.map[pointerX][y].GetComponent<LetterTile>().GetLetterPrice();
                        keyHorizontalWord += pointerX.ToString() + ";" + y.ToString();
                        horizonralTileWord.tiles.Add(new Vector2Int(pointerX, y));
                        pointerX++;
                    }

                    int pointerY = y - 1;
                    while (pointerY >= 0 && IsTileRaw(mapGenerator.map[x][pointerY].GetComponent<LetterTile>()))
                        pointerY--;
                    pointerY++;

                    while (pointerY < mapGenerator.mapSizeY && IsTileRaw(mapGenerator.map[x][pointerY].GetComponent<LetterTile>()))
                    {
                        verticalWord += mapGenerator.map[x][pointerY].GetComponent<LetterTile>().letter;
                        verticalScore += mapGenerator.map[x][pointerY].GetComponent<LetterTile>().GetLetterPrice();
                        keyVerticalWord += x.ToString() + ";" + pointerY.ToString();
                        verticalTileWord.tiles.Add(new Vector2Int(x, pointerY));
                        pointerY++;
                    }

                    if (!isCheckWord.ContainsKey(keyHorizontalWord))
                        countScore += horizontalScore;

                    if (!isCheckWord.ContainsKey(keyVerticalWord))
                        countScore += verticalScore;

                    if (!(wordDictionary.checkWord(horizontalWord) && wordDictionary.checkWord(verticalWord) && (horizontalWord.Length > 1 || verticalWord.Length > 1)))
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
        if (personManager.persons[idPlayingPerson].id == PhotonNetwork.LocalPlayer.ActorNumber)
        //if(personManager.persons[idPlayingPerson].id == me.id)
            {
            List<TileWord> words = CheckWords(out addedScore);
            wordIsFind = words != null;

            if(wordIsFind)
            {
                if (addedScore == 1)
                    scoresText.text = "(" + addedScore.ToString() + " очко)";
                else if(addedScore >=2 && addedScore <= 4)
                    scoresText.text = "(" + addedScore.ToString() + " очка)";
                else
                    scoresText.text = "(" + addedScore.ToString() + " очков)";

                acceptWordButton.SetActive(true);
                acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(1, timeToAppearanceAcceptWordButton);
            }
            else
            {
                acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(0, timeToAppearanceAcceptWordButton).setOnComplete(OnCompleteAnitaion, acceptWordButton);
            }
        }
    }

    void Awake()
    {
        avatarSprites = GameObject.FindGameObjectWithTag(avatarSpritesTag);
        iconsContent = avatarSprites.GetComponent<IconsContent>();
        //icons = iconsContent.icons;

        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        instanceColorHolder = colorsHolder.GetComponent<ColorsHolder>();

        //personManager.connectPerson(me);
        /*
        personManager.connectPerson(new Person(1, "ThEnd", new Color(1, 0.6f, 0.6f, 1), 1));
        personManager.connectPerson(new Person(2, "DotaKot", new Color(0.6f, 1, 0.6f, 1), 2));
        personManager.connectPerson(new Person(3, "SpectreSpect", new Color(0.6f, 0.6f, 1, 1), 3));
        personManager.connectPerson(new Person(4, "Hexumee", new Color(1, 0.6f, 1, 1), 4));*/
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            personManager.connectPerson(new Person(
                player.ActorNumber - 1, 
                player.NickName,
                instanceColorHolder.colors[(int)player.CustomProperties["playerColorIndex"]],
                (int)player.CustomProperties["playerIconId"]));
        }



        //personManager.persons[1].score = 200;

        wordDictionary = new NetSpellDictionary();
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
