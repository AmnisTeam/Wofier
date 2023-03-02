using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : BaseRaw
{
    public int iconId;
    public UnityEngine.Color color;
    public string nickname;
    public List<Region> claimedRegions = new List<Region>();

    public Player()
    {

    }

    public Player(int id, string nickname, UnityEngine.Color color, int iconId)
    //public Player(int id, int iconId, UnityEngine.Color color, string nickname)
    {
        /*
        this.id = id;
        this.iconId = iconId;
        this.color = color;
        this.nickname = nickname;*/
        this.id = id;
        this.nickname = nickname;
        this.color = color;
        this.iconId = iconId;
    }

    public void ClaimRegion(Region region)
    {
        claimedRegions.Add(region);
    }
}

public class PlayerAnswerData : BaseRaw
{
    public int answerId;
    public float timeToAnswer;
}
public class PlayersManager : MonoBehaviourPunCallbacks
{
    public int MAX_COUNT_PLAYERS = 4;
    public BaseTable<Player> players = new BaseTable<Player>();
    public BaseTable<PlayerAnswerData> playerAnswerData;
    public ConfigTemp config;
    private TabMenuManager tabMenuManager;
    public ToastShower toastShower;

    public string avatarSpritesTag;
    GameObject avatarSprites;
    IconsContent iconsContent;
    //Sprite[] icons;

    public string colorsHolderTag;
    private GameObject colorsHolder;
    private ColorsHolder instanceColorHolder;

    public void connected(Player player)
    {
        players.add(player);
        playerAnswerData.addwid(new PlayerAnswerData(), player);
        Debug.Log("Player " + player.nickname + " has been connected!");
        tabMenuManager.updateTabMenu();
    }

    public void disconnect(Player player)
    {
        int id = 0;
        for(int x = 0; x < players.count; x++)
            if(players.get(x) == player)
            {
                id = x;
                break;
            }
        toastShower.showText("Игрок " + player.nickname + " покинул игру.");
        players.list.RemoveAt(id);
        tabMenuManager.disconnectPlayer(id);
    }

    public void disconnect(int id)
    {
        toastShower.showText("Игрок " + players.get(id).nickname + " покинул игру.");
        players.list.RemoveAt(id);
        tabMenuManager.disconnectPlayer(id);
    }

    /*
    public override void OnLeftRoom()
    {
        disconnect(PhotonNetwork.LocalPlayer.ActorNumber - 1);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        disconnect(otherPlayer.ActorNumber - 1);
    }*/

    void Start()
    {
        tabMenuManager = GetComponent<TabMenuManager>();
        playerAnswerData = new BaseTable<PlayerAnswerData>();

        avatarSprites = GameObject.FindGameObjectWithTag(avatarSpritesTag);
        iconsContent = avatarSprites.GetComponent<IconsContent>();
        //icons = iconsContent.icons;

        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        instanceColorHolder = colorsHolder.GetComponent<ColorsHolder>();
        /*
        connected(new Player(0, "SpectreSpect",  new UnityEngine.Color(255, 0, 0), 0));
        connected(new Player(1, "DotaKot", new UnityEngine.Color(0, 255, 0), 1));
        connected(new Player(2, "ThEnd", new UnityEngine.Color(0, 0, 255), 2));
        connected(config.me);*/
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            connected(new Player(
                player.ActorNumber - 1,
                player.NickName,
                instanceColorHolder.colors[(int)player.CustomProperties["playerColorIndex"]],
                (int)player.CustomProperties["playerIconId"]));
        }
    }

    void Update()
    {

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
