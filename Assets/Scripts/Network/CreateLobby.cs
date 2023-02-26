using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayersInfo;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateLobby : MonoBehaviourPunCallbacks
{
    public string lobbbySceneName;

    public TextMeshProUGUI lobbyNameTMP;
    public TextMeshProUGUI lobbyPasswordTMP;
    public IconScroller iconScroller;
    public CreateLobbySaveData createLobbySaveData;

    private string lobbyName;
    private string lobbyPassword;
    private string lobbyIconID;

    public string colorsHolderTag;
    private GameObject colorsHolder;

    public string playersInfoTag;
    private GameObject playersInfo;
    private PlayersInfo inctanceplayersInfo;

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Вы не можете создать лобби так как не подключены к серверу");
            return;
        }

        playersInfo = GameObject.FindGameObjectWithTag(playersInfoTag);
        inctanceplayersInfo = playersInfo.GetComponent<PlayersInfo>();



        /*
        System.Random rnd = new System.Random();
        string code = rnd.Next(100000, 999999).ToString();

        string alhp = "abcdefghijklmnopqrstuvwxyz";
        string code = "";
        System.Random rnd = new System.Random();
        for (int i = 0; i < 6; i++)
            code += alhp[rnd.Next(0, alhp.Length)];*/

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        /*
        roomOptions.EmptyRoomTtl = 60000;
        roomOptions.PlayerTtl = 60000;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;*/

        lobbyName = lobbyNameTMP.text;
        lobbyPassword = lobbyPasswordTMP.text;
        lobbyIconID = iconScroller.selectedId.ToString();
        /*
        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        ColorsHolder inctanceColorHolder = colorsHolder.GetComponent<ColorsHolder>();
        List<int> freeColorsIdxList = inctanceColorHolder.freeColorsIdx;*/




        //PhotonNetwork.LocalPlayer.CustomProperties = hash;


        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
        setValue.Add("lobbyName", lobbyName);
        setValue.Add("lobbyPassword", lobbyPassword);
        setValue.Add("lobbyIconID", lobbyIconID);
        setValue.Add("lobbyCode", lobbyName);

        roomOptions.CustomRoomProperties = setValue;

        PhotonNetwork.CreateRoom(lobbyName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);

        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        ColorsHolder colors = colorsHolder.GetComponent<ColorsHolder>();

        ExitGames.Client.Photon.Hashtable info = new ExitGames.Client.Photon.Hashtable
        {
            { "nickname", PhotonNetwork.NickName },
            { "iconID", data.iconID },
            { "color", colors.GetRandomIdx() }
        };

        SendCustomClassWithRPC(info);

        PhotonNetwork.LoadLevel(lobbbySceneName);
        Debug.Log("Создана комната: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Ошибка создания лобби");
        Debug.Log(returnCode);
        Debug.Log(message);
    }

    void SendCustomClassWithRPC(ExitGames.Client.Photon.Hashtable classObj)
    {
        playersInfo.GetComponent<PhotonView>().RPC("SendPlayerInfo", RpcTarget.AllBuffered, classObj);
        //playersInfo.GetComponent<PhotonView>().RPC("AddPlayer", RpcTarget.AllBuffered);
    }

    /*
    PlayersInfo.Info ReceiveCustomClass(object serializedClass)
    {
        
        PlayersInfo.Info deserializedClass = new PlayersInfo.Info();
        deserializedClass = (PlayersInfo.Info)serializedClass;
        

        return (PlayersInfo.Info)serializedClass;
    }
    */



    /*
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Попал в функцию OnRoomListUpdate");
        foreach (RoomInfo room in roomList)
        {
            Debug.Log("Сущетвует комната: " + room.Name);
        }

    }*/
}
