using Photon.Pun;
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
    public GameObject playerCointainer;
    public GameObject playerPrefab;

    public Transform[] spawnPoints;
    private int spawnIndex = 0;

    public string avatarSpritesTag;
    GameObject avatarSprites;
    IconsContent iconsContent;
    Sprite[] icons;

    public string colorsHolderTag;
    private GameObject colorsHolder;
    private ColorsHolder inctanceColorHolder;
    List <Color32> colors;

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

        ColorsHolder.instance.refillFreeIndicies();

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (!player.IsLocal)
            {
                ColorsHolder.instance.freeColorsIdx.Remove((int)player.CustomProperties["playerColorIndex"]);
            }
            else
            {
                var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);
                var colorIndex = ColorsHolder.instance.getRandomIndex();
                var color = ColorsHolder.instance.freeColorsIdx[colorIndex];
                ColorsHolder.instance.freeColorsIdx.Remove(color);

                player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable{
                    {"playerColorIndex", color},
                    {"playerIconId", data.iconID}
                });
            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("»грок " + newPlayer.NickName + " зашЄл");
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log(">>> LobbyManager.OnPlayerPropertiesUpdate");
        UpdateList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("»грок " + otherPlayer.NickName + " вышел");
        UpdateList();
    }


    public void LeaveLobbyFunc()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("ConnectToServer");
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
            remotePlayerColor.color = ColorsHolder.instance.colors[(int)player.CustomProperties["playerColorIndex"]];
            remotePlayerIcon.sprite = icons[(int)player.CustomProperties["playerIconId"]];
            remotePlayerIcon.color = ColorsHolder.instance.colors[(int)player.CustomProperties["playerColorIndex"]];

            if (player.IsMasterClient && player.IsLocal)
            {
                remotePlayerButton.gameObject.SetActive(false);
            }
            else if (player.IsMasterClient && !player.IsLocal)
            {
                remotePlayerButton.gameObject.SetActive(false);
            }
            else if (player.IsLocal)
            {
                remotePlayerButton.gameObject.SetActive(false);
            }

            spawnIndex++;
        }
    }

}
