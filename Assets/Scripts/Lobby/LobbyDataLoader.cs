using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDataLoader : MonoBehaviour
{
    public GameObject lobbyGameObject;
    Image lobbyIcon;
    public TextMeshProUGUI lobbyName;
    public TextMeshProUGUI lobbyCode;
    public TextMeshProUGUI lobbyPassword;

    void Start()
    {
        lobbyIcon = lobbyGameObject.GetComponent<Image>();

        //lobbyIcon.sprite = CreateLobbyDataHolder.icons[CreateLobbyDataHolder.lobbyIconID];
        //lobbyName.text = CreateLobbyDataHolder.lobbyName;
        //lobbyCode.text = CreateLobbyDataHolder.lobbyCode;
        //lobbyPassword.text = CreateLobbyDataHolder.lobbyPassword;

        lobbyCode.text = PhotonNetwork.CurrentRoom.CustomProperties["lobbyCode"].ToString();
        lobbyIcon.sprite = PhotonNetwork.CurrentRoom.CustomProperties["lobbyIconID"];
        lobbyName.text = PhotonNetwork.CurrentRoom.CustomProperties["lobbyName"].ToString();
        lobbyPassword.text = PhotonNetwork.CurrentRoom.CustomProperties["lobbyPassword"].ToString();
    }
}
