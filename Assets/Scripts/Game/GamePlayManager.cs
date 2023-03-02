using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
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
    public Inventory inventory;

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
    public List<TileWord> findWords;

    public int idPlayingPerson = -1;
    public int numberOfStep = -1;

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
    public WordChecker wordChecker;

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

    public void AcceptWord()
    {
        if(wordIsFind)
        {
            int countTiles = 0;
            for(int x = 0; x < findWords.Count; x++)
            {
                for(int y = 0; y < findWords[x].tiles.Count; y++)
                {
                    LetterTile tile = mapGenerator.map[findWords[x].tiles[y].x][findWords[x].tiles[y].y].GetComponent<LetterTile>();
                    if (tile)
                    {
                        tile.inWord = true;
                        countTiles++;
                    }
                }

            }

            numberOfStep++;
            inventory.AddRandomLetters(countTiles);

            acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(0, timeToAppearanceAcceptWordButton).setOnComplete(OnCompleteAnitaion, acceptWordButton);
            SelectNextPersonToPlay();
        }
    }


    void Awake()
    {
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
