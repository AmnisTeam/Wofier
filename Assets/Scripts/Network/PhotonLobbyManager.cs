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

    public string playersInfoTag;
    private GameObject playersInfo;
    private PlayersInfo inctanceplayersInfo;
    List<PlayersInfo.Info> pInfo;

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

        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        inctanceColorHolder = colorsHolder.GetComponent<ColorsHolder>();
        colors = inctanceColorHolder.colors;

        playersInfo = GameObject.FindGameObjectWithTag(playersInfoTag);
        inctanceplayersInfo = playersInfo.GetComponent<PlayersInfo>();
        pInfo = inctanceplayersInfo.playersInfo;

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            //GameObject tempListing = Instantiate(playerPrefab, spawnPoints[spawnIndex]);
            GameObject tempListing = PhotonNetwork.Instantiate("Player", new Vector3(0,0,0), Quaternion.identity);
            tempListing.transform.SetParent(spawnPoints[spawnIndex].transform, false);

            Image playerIcon = tempListing.transform.GetChild(1).GetComponent<Image>();
            TextMeshProUGUI tempText = tempListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Image playerImg = tempListing.transform.GetChild(3).GetComponent<Image>();

            playerIcon.sprite = icons[pInfo[spawnIndex].avatarID];                      //icon
            playerIcon.color = colors[pInfo[spawnIndex].color];                         //icon color
            playerImg.color = colors[pInfo[spawnIndex].color];                          //player color
            tempText.text = player.NickName;                                            //player nickname

            spawnIndex++;
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("»грок " + newPlayer.NickName + " зашЄл");

        //GameObject tempListing = Instantiate(playerPrefab, spawnPoints[spawnIndex]);
        GameObject tempListing = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
        tempListing.transform.SetParent(spawnPoints[spawnIndex].transform, false);

        Image playerIcon = tempListing.transform.GetChild(1).GetComponent<Image>();
        TextMeshProUGUI tempText = tempListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Image playerImg = tempListing.transform.GetChild(3).GetComponent<Image>();

        playerIcon.sprite = icons[pInfo[spawnIndex].avatarID];                      //icon
        playerIcon.color = colors[pInfo[spawnIndex].color];                         //icon color
        playerImg.color = colors[pInfo[spawnIndex].color];                          //player color
        tempText.text = newPlayer.NickName;                                         //player nickname

        spawnIndex++;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("»грок " + otherPlayer.NickName + " вышел");

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
            //GameObject tempListing = Instantiate(playerPrefab, spawnPoints[spawnIndex]);
            GameObject tempListing = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
            tempListing.transform.SetParent(spawnPoints[spawnIndex].transform, false);

            Image playerIcon = tempListing.transform.GetChild(1).GetComponent<Image>();
            TextMeshProUGUI tempText = tempListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Image playerImg = tempListing.transform.GetChild(3).GetComponent<Image>();

            playerIcon.sprite = icons[pInfo[spawnIndex].avatarID];                      //icon
            playerIcon.color = colors[pInfo[spawnIndex].color];                         //icon color
            playerImg.color = colors[pInfo[spawnIndex].color];                          //player color
            tempText.text = player.NickName;                                            //player nickname

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

/*   
    private void Update()
    {
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

            playerIcon.sprite = icons[pInfo[spawnIndex].avatarID];                      //icon
            playerIcon.color = colors[pInfo[spawnIndex].color];                         //icon color
            playerImg.color = colors[pInfo[spawnIndex].color];                          //player color
            tempText.text = player.NickName;                                            //player nickname

            spawnIndex++;
        }
    }*/

    /*    
    public int RandColorIdx()
    {
        RoomOptions roomOptions = new RoomOptions();
        var freeColorsIdxFromRoomProperties = roomOptions.CustomRoomProperties;
        List<int> freeColorsIdxListFromRoomProperties = (List<int>)freeColorsIdxFromRoomProperties["freeColorsIdx"];

        var rnd = new System.Random();
        var r = rnd.Next(0, freeColorsIdxListFromRoomProperties.Count);

        int randColorIdx = freeColorsIdxListFromRoomProperties[r];
        freeColorsIdxListFromRoomProperties.RemoveAt(r);

        return randColorIdx;
    }*/

}
