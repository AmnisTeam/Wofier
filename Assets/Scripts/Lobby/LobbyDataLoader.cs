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
        var roomName = PhotonNetwork.CurrentRoom.Name;
        Debug.Log($"Joined room {roomName}");

        lobbyIcon = lobbyGameObject.GetComponent<Image>();

        lobbyIcon.sprite = CreateLobbyDataHolder.icons[CreateLobbyDataHolder.lobbyIconID];
        lobbyName.text = CreateLobbyDataHolder.lobbyName;
        lobbyCode.text = PhotonNetwork.CurrentRoom.Name;
        lobbyPassword.text = CreateLobbyDataHolder.lobbyPassword;
    }
}
