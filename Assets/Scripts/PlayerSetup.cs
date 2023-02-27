using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myChracter;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            //var data = SaveManager.Load<SaveData>(ConfigManager.saveKey);

            //colorsHolder = GameObject.FindGameObjectWithTag(colorsHolderTag);
            //ColorsHolder colors = colorsHolder.GetComponent<ColorsHolder>();

            //ExitGames.Client.Photon.Hashtable info = new ExitGames.Client.Photon.Hashtable
            //{
            //    { "nickname", PhotonNetwork.NickName },
            //    { "iconID", data.iconID },
            //    { "color", colors.GetRandomIdx() }
            //};
            //PV.RPC("test", RpcTarget.AllBuffered, )
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
