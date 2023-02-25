using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class JoinLobby : MonoBehaviourPunCallbacks
{
    public string lobbbySceneName;

    public TextMeshProUGUI lobbyCodeTMP;
    public TextMeshProUGUI lobbyPasswordTMP;

    public PlayerManager playerManager;

    public void JoinLobbyFunc()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("�� �� ������ ������� ����� ��� ��� �� ���������� � �������");
            return;
        }

        Debug.Log("����������� � �����: " + lobbyCodeTMP.text);
        PhotonNetwork.JoinRoom(lobbyCodeTMP.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(lobbbySceneName);
        Debug.Log("�� �������������� � �������: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(returnCode);
        Debug.Log(message);
    }

}
