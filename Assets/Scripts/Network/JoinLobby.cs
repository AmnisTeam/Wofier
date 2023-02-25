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

        Debug.Log("Поделючение к лобби: " + lobbyCodeTMP.text);
        PhotonNetwork.JoinRoom(lobbyCodeTMP.text);
    }

    public override void OnJoinedRoom()
    {
        var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);

        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        ColorsHolder colors = colorsHolder.GetComponent<ColorsHolder>();
        Color32 randColor = colors.GetRandomColor();

        Hashtable hash = new Hashtable();
        hash.Add("nickname", PhotonNetwork.NickName);
        hash.Add("iconID", data.iconID);
        hash.Add("color", randColor);
        //hash.Add("r", Convert.ToByte(randColor.r));
        //hash.Add("g", Convert.ToByte(randColor.g));
        //hash.Add("b", Convert.ToByte(randColor.b));

        PhotonNetwork.MasterClient.CustomProperties = hash;

        PhotonNetwork.LoadLevel(lobbbySceneName);
        Debug.Log("Вы присоеденились к комнате: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(returnCode);
        Debug.Log(message);
    }

}
