using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRoomList : MonoBehaviourPunCallbacks
{
    public Transform content;
    public PhotonRoom roomPrefab;

    private void Start()
    {
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            PhotonRoom room = Instantiate(roomPrefab, content);
            if (room != null )
            {
                room.SetRoomInfo(roomInfo);
            }
        }
    }
}
