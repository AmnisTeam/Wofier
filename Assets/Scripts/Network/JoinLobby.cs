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

    public void JoinLobbyFunc()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Вы не можете создать лобби так как не подключены к серверу");
            return;
        }

        //colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        //ColorsHolder colors = colorsHolder.GetComponent<ColorsHolder>();
        //int randColorIdx = colors.GetRandomIdx();



        Debug.Log("Подключение к лобби: " + lobbyCodeTMP.text);
        PhotonNetwork.JoinRoom(lobbyCodeTMP.text);
        var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);
        Hashtable hash = new Hashtable();
        hash.Add("nickname", PhotonNetwork.NickName);
        hash.Add("iconID", data.iconID);
        hash.Add("colorIdx", RandColorIdx());

        PhotonNetwork.LocalPlayer.CustomProperties = hash;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(lobbbySceneName);
        Debug.Log("Вы присоеденились к комнате: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
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

}
