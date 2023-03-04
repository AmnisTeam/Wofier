using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static PlayersInfo;
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
    /*
    public string playersInfoTag;
    private GameObject playersInfo;
    private PlayersInfo inctanceplayersInfo;*/

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("�� �� ������ ������� ����� ��� ��� �� ���������� � �������");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        lobbyName = lobbyNameTMP.text;
        lobbyPassword = lobbyPasswordTMP.text;
        lobbyIconID = iconScroller.selectedId.ToString();


        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable
        {
            { "lobbyName", lobbyName },
            { "lobbyPassword", lobbyPassword },
            { "lobbyIconID", lobbyIconID },
            { "lobbyCode", lobbyName }
        };

        roomOptions.CustomRoomProperties = setValue;

        PhotonNetwork.CreateRoom(lobbyPassword, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        /*
        var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);

        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        ColorsHolder colors = colorsHolder.GetComponent<ColorsHolder>();

        ExitGames.Client.Photon.Hashtable info = new ExitGames.Client.Photon.Hashtable
        {
            { "nickname", PhotonNetwork.NickName },
            { "iconID", data.iconID },
            { "color", colors.GetRandomIdx() }
        };

        SendCustomClassWithRPC(info);*/

        PhotonNetwork.LoadLevel(lobbbySceneName);
        Debug.Log("������� �������: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("������ �������� �����");
        Debug.Log(returnCode);
        Debug.Log(message);
    }
    /*
    void SendCustomClassWithRPC(ExitGames.Client.Photon.Hashtable classObj)
    {
        playersInfo.GetComponent<PhotonView>().RPC("SendPlayerInfo", RpcTarget.AllBuffered, classObj);
    }*/

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
        Debug.Log("����� � ������� OnRoomListUpdate");
        foreach (RoomInfo room in roomList)
        {
            Debug.Log("��������� �������: " + room.Name);
        }

    }*/
}
