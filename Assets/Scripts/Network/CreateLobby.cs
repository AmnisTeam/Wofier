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


    private string lobbyName;
    private string lobbyPassword;
    private string lobbyIconID;


    public void CreateRoom()
    {

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        lobbyName = lobbyNameTMP.text;
        lobbyPassword = lobbyPasswordTMP.text;
        lobbyIconID = iconScroller.selectedId.ToString();

        roomOptions.CustomRoomPropertiesForLobby = new string[] { "lobbyName", "lobbyPassword", "lobbyIconID" };
        roomOptions.CustomRoomProperties = new Hashtable 
        { { "lobbyName", lobbyName }, { "lobbyPassword", lobbyPassword }, {"lobbyIconID", lobbyIconID } };

        System.Random rnd = new System.Random();
        int value = rnd.Next(100000, 999999);

        PhotonNetwork.CreateRoom(value.ToString(), roomOptions, null);
        //PhotonNetwork.JoinRoom(value.ToString());
        Debug.Log(value.ToString());


        //lobbyIcon = lobbyGameObject.GetComponent<Image>();
        //lobbyIcon.sprite = CreateLobbyDataHolder.icons[CreateLobbyDataHolder.lobbyIconID];

        //PhotonNetwork.CreateRoom()
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel(lobbbySceneName);
    }
    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //}
}
