using Mirror.Examples.MultipleMatch;
using Photon.Pun;
using Photon.Realtime;
using System;
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

    public string colorsHolderTag;
    private GameObject colorsHolder;

    public string playersInfoTag;
    private GameObject playersInfo;
    private PlayersInfo inctanceplayersInfo;

    public void JoinLobbyFunc()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("�� �� ������ ������� ����� ��� ��� �� ���������� � �������");
            return;
        }

        playersInfo = GameObject.FindGameObjectWithTag(playersInfoTag);
        inctanceplayersInfo = playersInfo.GetComponent<PlayersInfo>();


        Debug.Log("����������� � �����: " + lobbyCodeTMP.text);
        PhotonNetwork.JoinRoom(lobbyCodeTMP.text);
    /*
        var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);
        Hashtable hash = new Hashtable();
        hash.Add("nickname", PhotonNetwork.NickName);
        hash.Add("iconID", data.iconID);
        hash.Add("colorIdx", RandColorIdx());

        PhotonNetwork.LocalPlayer.CustomProperties = hash;*/

    }

    public override void OnJoinedRoom()
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
        Debug.Log("�� �������������� � �������: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(returnCode);
        Debug.Log(message);
    }

    void SendCustomClassWithRPC(ExitGames.Client.Photon.Hashtable classObj)
    {
        playersInfo.GetComponent<PhotonView>().RPC("SendPlayerInfo", RpcTarget.AllBuffered, classObj);
    }

}
