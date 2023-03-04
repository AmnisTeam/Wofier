using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonLobbyManager : MonoBehaviourPunCallbacks
{
    public string mainSceneName;
    public GameObject playerPrefab;
    public GameObject startButtonGameObject;

    public Transform[] spawnPoints;
    private int spawnIndex = 0;

    public string avatarSpritesTag;
    GameObject avatarSprites;
    IconsContent iconsContent;
    Sprite[] icons;

    public string colorsHolderTag;
    private GameObject colorsHolder;
    private ColorsHolder instanceColorHolder;

    public string gameScneneName;


    /*
    public string playersInfoTag;
    private GameObject playersInfo;
    private PlayersInfo inctanceplayersInfo;
    List<PlayersInfo.Info> pInfo;

    border
    icon
    nickname
    color
    button*/

    void Start()
    {
        avatarSprites = GameObject.FindGameObjectWithTag(avatarSpritesTag);
        iconsContent = avatarSprites.GetComponent<IconsContent>();
        icons = iconsContent.icons;

        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        instanceColorHolder = colorsHolder.GetComponent<ColorsHolder>();

        instanceColorHolder.refillFreeIndicies();

        //ColorsHolder.instance.refillFreeIndicies();

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (!player.IsLocal)
            {
                instanceColorHolder.freeColorsIdx.Remove((int)player.CustomProperties["playerColorIndex"]);
            }
            else
            {
                var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);
                var colorIndex = instanceColorHolder.getRandomIndex();
                var color = instanceColorHolder.freeColorsIdx[colorIndex];
                instanceColorHolder.freeColorsIdx.Remove(color);

                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable{
                    {"playerColorIndex", color},
                    {"playerIconId", data.iconID}
                });
            }
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            startButtonGameObject.gameObject.SetActive(false);
        }
    }
    public override void OnJoinedRoom()
    {
        var s = PhotonNetwork.CurrentRoom.CustomProperties;

        //if (s["lobbyPassword"].ToString().Equals("qwes"))
        if (true)
        {
            Debug.Log("эта хрень работает");
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Игрок " + newPlayer.NickName + " зашёл");
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log(">>> LobbyManager.OnPlayerPropertiesUpdate");
        UpdateList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Игрок " + otherPlayer.NickName + " вышел");
        UpdateList();
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(gameScneneName);
    }

    public void LeaveLobbyFunc()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    public void UpdateList()
    {
        foreach (Transform spawn in spawnPoints)
        {
            for (var i = spawn.childCount - 1; i >= 0; i--)
            {
                PhotonNetwork.Destroy(spawn.GetChild(i).gameObject);
                spawnIndex--;
            }
        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject remotePlayerObject = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, Quaternion.identity);
            remotePlayerObject.transform.SetParent(spawnPoints[spawnIndex].transform, false);

            TextMeshProUGUI remoteTempText = remotePlayerObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Image remotePlayerColor = remotePlayerObject.transform.GetChild(3).GetComponent<Image>();
            Image remotePlayerIcon = remotePlayerObject.transform.GetChild(1).GetComponent<Image>();
            Button remotePlayerButton = remotePlayerObject.transform.GetChild(4).GetComponent<Button>();

            remoteTempText.text = player.NickName;
            remotePlayerColor.color = instanceColorHolder.colors[(int)player.CustomProperties["playerColorIndex"]];
            remotePlayerIcon.sprite = icons[(int)player.CustomProperties["playerIconId"]];
            remotePlayerIcon.color = instanceColorHolder.colors[(int)player.CustomProperties["playerColorIndex"]];

            remotePlayerButton.onClick.AddListener(() => {
                PhotonNetwork.CloseConnection(player);
            });

            if (!PhotonNetwork.IsMasterClient || (PhotonNetwork.IsMasterClient && player.IsLocal))
            {
                remotePlayerButton.gameObject.SetActive(false);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                startButtonGameObject.gameObject.SetActive(true);
            }

            spawnIndex++;
        }
    }

}
