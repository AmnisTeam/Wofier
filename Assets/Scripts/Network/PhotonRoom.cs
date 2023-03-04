using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonRoom : MonoBehaviour
{
    public TextMeshProUGUI roomName;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.Name + ", " + roomInfo.MaxPlayers;
    }
}
