using Photon.Pun;
using Photon.Realtime;
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


    public void CreateRoom()
    {
        System.Random rnd = new System.Random();
        int value = rnd.Next(100000, 999999);
        string code = "#" + value.ToString();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        lobbyName = lobbyNameTMP.text;
        lobbyPassword = lobbyPasswordTMP.text;
        lobbyIconID = iconScroller.selectedId.ToString();

        roomOptions.CustomRoomPropertiesForLobby = new string[] { "lobbyName", "lobbyPassword", "lobbyIconID", "lobbyCode" };
        roomOptions.CustomRoomProperties = new Hashtable
        { { "lobbyName", lobbyName }, { "lobbyPassword", lobbyPassword }, {"lobbyIconID", lobbyIconID }, {"lobbyCode", code} };



        PhotonNetwork.CreateRoom(code, roomOptions, null);

        Debug.Log(code);

    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel(lobbbySceneName);
    }
}
