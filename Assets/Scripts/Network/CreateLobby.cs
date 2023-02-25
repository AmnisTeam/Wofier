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
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Вы не можете создать лобби так как не подключены к серверу");
            return;
        }

        //System.Random rnd = new System.Random();
        //string code = rnd.Next(100000, 999999).ToString();
        string alhp = "abcdefghijklmnopqrstuvwxyz";
        string code = "";
        System.Random rnd = new System.Random();
        for (int i = 0; i < 6; i++)
            code += alhp[rnd.Next(0, alhp.Length)];

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        //roomOptions.EmptyRoomTtl = 60000;
        //roomOptions.PlayerTtl = 60000;
        //roomOptions.IsOpen = true;
        //roomOptions.IsVisible = true;

        lobbyName = lobbyNameTMP.text;
        lobbyPassword = lobbyPasswordTMP.text;
        lobbyIconID = iconScroller.selectedId.ToString();

        roomOptions.CustomRoomPropertiesForLobby = new string[] { "lobbyName", "lobbyPassword", "lobbyIconID", "lobbyCode" };
        roomOptions.CustomRoomProperties = new Hashtable
        { { "lobbyName", lobbyName }, { "lobbyPassword", lobbyPassword }, 
          {"lobbyIconID", lobbyIconID }, {"lobbyCode", lobbyName} };



        PhotonNetwork.CreateRoom(lobbyName, roomOptions, TypedLobby.Default);
        

        //Debug.Log(code);

    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel(lobbbySceneName);
        Debug.Log("Количество комнат: " + PhotonNetwork.CountOfRooms.ToString());
        Debug.Log("Создана комната: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Количество комнат: " + PhotonNetwork.CountOfRooms.ToString());
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Ошибка создания лобби");
        Debug.Log(returnCode);
        Debug.Log(message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Попал в функцию OnRoomListUpdate");
        foreach (RoomInfo room in roomList)
        {
            Debug.Log("Сущетвует комната: " + room.Name);
        }

    }

}
