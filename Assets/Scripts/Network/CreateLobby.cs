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
        int code = rnd.Next(100000, 999999);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.EmptyRoomTtl = 60000;
        roomOptions.PlayerTtl = 60000;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        lobbyName = lobbyNameTMP.text;
        lobbyPassword = lobbyPasswordTMP.text;
        lobbyIconID = iconScroller.selectedId.ToString();

        roomOptions.CustomRoomPropertiesForLobby = new string[] { "lobbyName", "lobbyPassword", "lobbyIconID", "lobbyCode" };
        roomOptions.CustomRoomProperties = new Hashtable
        { { "lobbyName", lobbyName }, { "lobbyPassword", lobbyPassword }, {"lobbyIconID", lobbyIconID }, {"lobbyCode", "123"} };



        PhotonNetwork.CreateRoom("123", roomOptions, TypedLobby.Default, null);
        

        Debug.Log(code);

    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CountOfRooms.ToString());
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel(lobbbySceneName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(returnCode);
        Debug.Log(message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("ssss");
        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
        }
        Debug.Log("ssss");

    }

}
