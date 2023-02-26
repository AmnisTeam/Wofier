using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayersInfo : MonoBehaviour
{
    public static PlayersInfo instance;

    [Serializable]
    public class Info
    {
        public string nickname;
        public int avatarID;
        public int color;

        public Info(string nickname, int avatarID, int color)
        {
            this.nickname = nickname;
            this.avatarID = avatarID;
            this.color = color;
        }
        public Info()
        {
            //this.nickname = nickname;
            //this.avatarID = avatarID;
            //this.color = color;
        }
    }

    public List<Info> playersInfo;
/*    public GameObject[] playersGameObject;

    public GameObject playerPrefab;

    public GameObject[] spawnPoints;
    private int spawnIndex = 0;

    public string avatarSpritesTag;
    GameObject avatarSprites;
    IconsContent iconsContent;
    Sprite[] icons;

    public string colorsHolderTag;
    private GameObject colorsHolder;
    private ColorsHolder inctanceColorHolder;
    List<Color32> colors;

        public string playersInfoTag;
        private GameObject playersInfoGO;
        private PlayersInfo inctanceplayersInfo;
        List<PlayersInfo.Info> pInfo;
        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                {
                    Destroy(instance.gameObject);
                    instance = this;
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }*/

    void Start()
    {
        playersInfo = new List<Info>();
    }



    [PunRPC]
    public void SendPlayerInfo(ExitGames.Client.Photon.Hashtable info)
    {
        List<PlayersInfo.Info> pInfo = this.playersInfo;
        pInfo.Add(new PlayersInfo.Info((string)info["nickname"], (int)info["iconID"], (int)info["color"]));
    }
/*
    [PunRPC]
    public void AddPlayer()
    {
        //spawnPoints[spawnIndex] = GameObject.FindGameObjectWithTag("SPAWN_1_TAG");
        //Scene scene = SceneManager.GetActiveScene();
        //Debug.Log(scene.name);
        GameObject tempListing = Instantiate(playerPrefab*//*, spawnPoints[spawnIndex].transform*//*);

        Image playerIcon = tempListing.transform.GetChild(1).GetComponent<Image>();
        TextMeshProUGUI tempText = tempListing.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Image playerImg = tempListing.transform.GetChild(3).GetComponent<Image>();

        avatarSprites = GameObject.FindGameObjectWithTag(avatarSpritesTag);
        iconsContent = avatarSprites.GetComponent<IconsContent>();
        icons = iconsContent.icons;

        colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
        inctanceColorHolder = colorsHolder.GetComponent<ColorsHolder>();
        colors = inctanceColorHolder.colors;

        playerIcon.sprite = icons[playersInfo[spawnIndex].avatarID];                      //icon
        playerIcon.color = colors[playersInfo[spawnIndex].color];                         //icon color
        playerImg.color = colors[playersInfo[spawnIndex].color];                          //player color
        tempText.text = playersInfo[spawnIndex].nickname;                                            //player nickname

        spawnIndex++;
    }*/
}
