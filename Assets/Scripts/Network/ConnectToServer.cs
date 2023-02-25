using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public string region;
    public string mainSceneName;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("�� ����������� �: " + PhotonNetwork.CloudRegion);
        //PhotonNetwork.JoinLobby();

        SceneManager.LoadScene(mainSceneName);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);
        Debug.Log("�� ��������� �� �������");
        Debug.Log(cause);
    }

    //public override void OnJoinedLobby()
    //{
    //    RoomOptions roomOptions = new RoomOptions();
    //    roomOptions.MaxPlayers = 4;
    //    PhotonNetwork.CreateRoom("qwe", roomOptions, TypedLobby.Default);
    //}
}
