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


    //border
    //icon
    //nickname
    //color
    //button


    private void Start()
    {
        avatarSprites = GameObject.FindGameObjectWithTag(avatarSpritesTag);
        iconsContent = avatarSprites.GetComponent<IconsContent>();
        icons = iconsContent.icons;

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerPrefab, spawnPoints[spawnIndex]);

            Image playerIcon = tempListing.transform.GetChild(1).GetComponent<Image>();
            TextMeshProUGUI tempText = tempListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Image playerImg = tempListing.transform.GetChild(3).GetComponent<Image>();

            //Color32 color = new Color32(
            //    (byte)player.CustomProperties["r"],
            //    (byte)player.CustomProperties["g"],
            //    (byte)player.CustomProperties["b"],
            //    255);
            Color32 color = (Color32)player.CustomProperties["color"];

            playerIcon.sprite = icons[(int)player.CustomProperties["iconID"]];  //icon
            playerIcon.color = color;                                           //icon color
            playerImg.color = color;                                            //player color
            tempText.text = player.NickName;                                    //player nickname

            spawnIndex++;
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("����� " + newPlayer.NickName + " �����");

        GameObject tempListing = Instantiate(playerPrefab, spawnPoints[spawnIndex]);

        Image playerIcon = tempListing.transform.GetChild(1).GetComponent<Image>();
        TextMeshProUGUI tempText = tempListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Image playerImg = tempListing.transform.GetChild(3).GetComponent<Image>();

        //Color32 color = new Color32(
        //    (byte)player.CustomProperties["r"],
        //    (byte)player.CustomProperties["g"],
        //    (byte)player.CustomProperties["b"],
        //    255);
        Color32 color = (Color32)newPlayer.CustomProperties["color"];

        playerIcon.sprite = icons[(int)newPlayer.CustomProperties["iconID"]];   //icon
        playerIcon.color = color;                                               //icon color
        playerImg.color = color;                                                //player color
        tempText.text = newPlayer.NickName;

        spawnIndex++;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("����� " + otherPlayer.NickName + " �����");

        //colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        //ColorsHolder colors = colorsHolder.GetComponent<ColorsHolder>();
        //Color32 oldColor;

        //colors.AddColor(oldColor);

        foreach (Transform spawn in spawnPoints)
        {
            for (var i = spawn.childCount - 1; i >= 0; i--)
            {
                Destroy(spawn.GetChild(i).gameObject);
                spawnIndex--;
            }
        }

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerPrefab, spawnPoints[spawnIndex]);

            Image playerIcon = tempListing.transform.GetChild(1).GetComponent<Image>();
            TextMeshProUGUI tempText = tempListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Image playerImg = tempListing.transform.GetChild(3).GetComponent<Image>();

            //Color32 color = new Color32(
            //    (byte)player.CustomProperties["r"],
            //    (byte)player.CustomProperties["g"],
            //    (byte)player.CustomProperties["b"],
            //    255);
            Color32 color = (Color32)player.CustomProperties["color"];

            playerIcon.sprite = icons[(int)player.CustomProperties["iconID"]];  //icon
            playerIcon.color = color;                                           //icon color
            playerImg.color = color;                                            //player color
            tempText.text = player.NickName;

            spawnIndex++;
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(mainSceneName);
    }

    public void LeaveLobbyFunc()
    {
        PhotonNetwork.LeaveRoom();
    }



}