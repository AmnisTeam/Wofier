using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.Demo.Shared.DocLinks;

public class TileWord
{
    public Person person = null;
    public List<Vector2Int> tiles;

    public TileWord()
    {
        tiles = new List<Vector2Int>();
    }
}

public class GamePlayManager : MonoBehaviour
{
    public Person me;
    public PersonsManager personManager;
    public ScoreTableManager scoreTableManager;
    public MapGenerator mapGenerator;
    public Inventory inventory;
    public PhotonView PV;

    public WordDictionary wordDictionary;

    public GameObject findingMenu;
    public TMPro.TMP_Text findingMenuNickname;

    public TMPro.TMP_Text timeNickname;
    public TMPro.TMP_Text time;

    public GameObject acceptWordButton;
    public GameObject acceptText;
    public TMPro.TMP_Text scoresText;

    public GameObject clue;

    public TMPro.TMP_Text stepsText;

    public bool wordIsFind = false;
    public float addedScore = 0;
    public List<TileWord> findWords;

    public int idPlayingPerson = -1;
    public int numberOfPlayerStep;
    public int gameSteps = 0;
    public int countStepsToEndGame = 40;

    public float timeToPlayingOnePerson;
    private float timerToPlayerOnePerson = float.NaN;

    public float timeToAppearanceFindingMenu;
    public float findingMenuShowingTime;
    private float findingMenuShowingTimer = float.NaN;

    public float timeToAppearanceClue;

    public float timeToAppearanceAcceptWordButton;

    public string avatarSpritesTag;
    GameObject avatarSprites;
    IconsContent iconsContent;

    public List<TileWord> words;

    public string colorsHolderTag;
    private GameObject colorsHolder;
    private ColorsHolder instanceColorHolder;
    public WordChecker wordChecker;
    public EndGameManager endGameManager;

    public void CheckToShowClueMenu()
    {
        if (numberOfPlayerStep == 0 && me.id == personManager.persons[idPlayingPerson].id)
        {
            clue.SetActive(true);
            clue.GetComponent<CanvasGroup>().LeanAlpha(1, timeToAppearanceClue);
        }
        else
        {
            clue.GetComponent<CanvasGroup>().LeanAlpha(0, timeToAppearanceClue).setOnComplete(() => {
                clue.SetActive(false);
            });
        }
    }

    public void SelectNextPersonToPlay()
    {
        idPlayingPerson = (idPlayingPerson + 1) % personManager.persons.Count;
        if(numberOfPlayerStep != 0)
            gameSteps++;
        stepsText.text = gameSteps + "/" + countStepsToEndGame;
        timerToPlayerOnePerson = timeToPlayingOnePerson;

        CheckToShowClueMenu();

        findingMenuNickname.text = personManager.persons[idPlayingPerson].nickname;
        findingMenuNickname.color = personManager.persons[idPlayingPerson].color;
        findingMenu.SetActive(true);
        findingMenu.GetComponent<CanvasGroup>().LeanAlpha(1, timeToAppearanceFindingMenu);
        findingMenuShowingTimer = findingMenuShowingTime;

        timeNickname.text = me.id == personManager.persons[idPlayingPerson].id ? "You" : personManager.persons[idPlayingPerson].nickname;
        timeNickname.color = personManager.persons[idPlayingPerson].color;

        scoreTableManager.updateTable();
    }

    public void OnCompleteAnitaion(object gameObject)
    {
        (gameObject as GameObject).SetActive(false);
    }

    public void AcceptWord()
    {
        if(wordIsFind)
        {
            int[][] chars = new int[findWords.Count][];
            int[][] coordX = new int[findWords.Count][];
            int[][] coordY = new int[findWords.Count][];
            int[][] personsID = new int[findWords.Count][];


            int[] completeWordTileX = new int[findWords.Count];
            int[] completeWordTileY = new int[findWords.Count];
            int[][] findWordX = new int[findWords.Count][];
            int[][] findWordY = new int[findWords.Count][];



            int countTiles = 0;
            for(int x = 0; x < findWords.Count; x++)
            {
                chars[x] = new int[findWords[x].tiles.Count];
                coordX[x] = new int[findWords[x].tiles.Count];
                coordY[x] = new int[findWords[x].tiles.Count];
                personsID[x] = new int[findWords[x].tiles.Count];

                findWordX[x] = new int[findWords[x].tiles.Count];
                findWordY[x] = new int[findWords[x].tiles.Count];

                LetterTile completeWordTile = null;

                for (int y = 0; y < findWords[x].tiles.Count; y++)
                {
                    LetterTile tile = mapGenerator.map[findWords[x].tiles[y].x][findWords[x].tiles[y].y].GetComponent<LetterTile>();

                    findWordX[x][y] = findWords[x].tiles[y].x;
                    findWordY[x][y] = findWords[x].tiles[y].y;


                    if (tile)
                    {
                        if (completeWordTile != null)
                        {
                            if (tile.completeWordOrder > completeWordTile.completeWordOrder)
                            {
                                completeWordTile = tile;
                                completeWordTileX[x] = findWords[x].tiles[y].x;
                                completeWordTileY[x] = findWords[x].tiles[y].y;
                            }
                        }
                        else
                        {
                            completeWordTile = tile;
                            completeWordTileX[x] = findWords[x].tiles[y].x;
                            completeWordTileY[x] = findWords[x].tiles[y].y;
                        }
                        tile.inWord = true;
                        countTiles++;

                        coordX[x][y] = findWords[x].tiles[y].x;
                        coordY[x][y] = findWords[x].tiles[y].y;
                        chars[x][y] = tile.letter;
                        personsID[x][y] = tile.person.id;
                    }
                }

                completeWordTile.CompleteWord(findWords[x]);
                words.Add(findWords[x]);
            }

            me.score += addedScore;
            PV.RPC("UpdateScore", RpcTarget.All, me.id, me.score);

            if (findWords.Count > 0)
            {
                inventory.mapGenerator.PV.RPC("UpdateWordOnAccept", RpcTarget.Others, coordX, coordY, 
                                              chars, personsID);
                inventory.mapGenerator.PV.RPC("UpdateCompletedWord", RpcTarget.Others, completeWordTileX, 
                                              completeWordTileY, findWordX, findWordY);
            }

            numberOfPlayerStep++;
            PV.RPC("UpdateStep", RpcTarget.All, numberOfPlayerStep);

            inventory.AddRandomLetters(countTiles);

            acceptWordButton.SetActive(false);
            acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(0, timeToAppearanceAcceptWordButton);
            PV.RPC("SelectNextPersonToPlayRPC", RpcTarget.All);
            PV.RPC("UpdateIdPlayingPerson", RpcTarget.Others, idPlayingPerson);
            PV.RPC("UpdateSteps", RpcTarget.Others, numberOfPlayerStep);
        }
    }
/*
    public void SetTimerToPlayerOnePerson(float f)
    {
        this.timerToPlayerOnePerson = f;
    }*/

    [PunRPC]
    public void SelectNextPersonToPlayRPC()
    {
        SelectNextPersonToPlay();
    }

    [PunRPC]
    public void UpdateIdPlayingPerson(int id)
    {
        idPlayingPerson = id;
    }
    [PunRPC]
    public void UpdateSteps(int step)
    {
        numberOfPlayerStep = step;
    }

    [PunRPC]
    public void UpdateStep(int step)
    {
        numberOfPlayerStep = step;
    }

    [PunRPC]
    public void UpdateScore(int playerID, float score)
    {
        Person person = null;
        for (int i = 0; i < personManager.persons.Count; i++)
        {
            if (playerID == personManager.persons[i].id)
            {
                person = personManager.persons[i];
                break;
            }
        }
        if (person != null)
        {
            person.score = score;
            scoreTableManager.updateTable();
        }
    }

    void Awake()
    {
        words = new List<TileWord>();
        stepsText.text = gameSteps + "/" + countStepsToEndGame;
        wordChecker = new WordChecker(this);
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
        me = personManager.persons[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        SelectNextPersonToPlay();
        PV = GetComponent<PhotonView>();
        me = personManager.persons[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        CheckToShowClueMenu();
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
            acceptWordButton.SetActive(false);
            acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(0, timeToAppearanceAcceptWordButton);
            SelectNextPersonToPlay();
        }

        int secundes = (int)(timerToPlayerOnePerson % 60);
        int minutes = (int)(timerToPlayerOnePerson / 60);

        time.text = minutes.ToString("00") + ":" + secundes.ToString("00");

        if(gameSteps >= countStepsToEndGame)
        {
            endGameManager.enabled = true;
            enabled = false;
            endGameManager.OpenEndGameMenu();
        }
    }

    /*
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.OpResponseReceived += NetworkingClientOnOpResponseReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.OpResponseReceived -= NetworkingClientOnOpResponseReceived;
    }

    private void NetworkingClientOnOpResponseReceived(OperationResponse opResponse)
    {
        if (opResponse.OperationCode == OperationCode.SetProperties &&
            opResponse.ReturnCode == ErrorCode.InvalidOperation)
        {
            // CAS failure
        }
    }*/

}
