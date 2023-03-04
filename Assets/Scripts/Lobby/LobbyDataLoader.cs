using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDataLoader : MonoBehaviour
{
    public string iconsTag;

    public GameObject lobbyGameObject;
    Image lobbyIcon;
    public TextMeshProUGUI lobbyName;
    public TextMeshProUGUI lobbyCode;
    public TextMeshProUGUI lobbyPassword;

    void Start()
    {
        GameObject iconsGameObject = GameObject.FindGameObjectWithTag(this.iconsTag);
        IconsContent iconsContentInstance = iconsGameObject.GetComponent<IconsContent>();
        Sprite[] icons = iconsContentInstance.icons;

        lobbyIcon = lobbyGameObject.GetComponent<Image>();

        lobbyCode.text = "#" + PhotonNetwork.CurrentRoom.CustomProperties["lobbyCode"].ToString();
        lobbyIcon.sprite = icons[Convert.ToInt32(PhotonNetwork.CurrentRoom.CustomProperties["lobbyIconID"])];
        lobbyIcon.color = new UnityEngine.Color(0.5f, 0.5f, 0.5f);
        lobbyName.text = PhotonNetwork.CurrentRoom.CustomProperties["lobbyName"].ToString();
        lobbyPassword.text = PhotonNetwork.CurrentRoom.CustomProperties["lobbyPassword"].ToString();
    }
}
