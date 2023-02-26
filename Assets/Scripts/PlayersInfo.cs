using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInfo : MonoBehaviour
{
    public static PlayersInfo instance;

    public PlayersInfo()
    {
        instance = this;
    }

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
    GameObject go;

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
}
