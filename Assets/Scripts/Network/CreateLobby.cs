using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Вы не можете создать лобби так как не подключены к серверу");
            return;
        }

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


        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        ColorsHolder inctanceColorHolder = colorsHolder.GetComponent<ColorsHolder>();
        int[] freeColorsIdxList = inctanceColorHolder.freeColorsIdx;

        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
        setValue.Add("lobbyName", lobbyName);
        setValue.Add("lobbyPassword", lobbyPassword);
        setValue.Add("lobbyIconID", lobbyIconID);
        setValue.Add("lobbyCode", lobbyName);
        setValue.Add("freeColorsIdxList", freeColorsIdxList);

        roomOptions.CustomRoomProperties = setValue;

        PhotonNetwork.CreateRoom(lobbyName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);
        Hashtable hash = new Hashtable();
        hash.Add("nickname", PhotonNetwork.NickName);
        hash.Add("iconID", data.iconID);
        hash.Add("colorIdx", RandColorIdx());

        PhotonNetwork.LocalPlayer.CustomProperties = hash;

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


    public int RandColorIdx()
    {
        var freeColorsIdxFromRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        int[] freeColorsIdxListFromRoomProperties = (int[])freeColorsIdxFromRoomProperties["freeColorsIdxList"];

        var rnd = new System.Random();
        var r = rnd.Next(0, freeColorsIdxListFromRoomProperties.Length);

        int randColorIdx = freeColorsIdxListFromRoomProperties[r];
        //freeColorsIdxListFromRoomProperties.RemoveAt(r);

        return randColorIdx;
    }



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
