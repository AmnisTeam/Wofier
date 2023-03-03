using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonsManager : MonoBehaviourPunCallbacks
{
    public List<Person> persons;
    public ScoreTableManager scoreTableManager;
    public GamePlayManager gamePlayManager;

    public void connectPerson(Person person)
    {
        persons.Add(person);
        scoreTableManager.updateTable();
        Debug.Log("Игрок " + person.nickname + " подключился к игре");
    }

    public void disconnectPerson(int personID)
    {
        Person person = null;
        int ID = 0;
        for (int i = 0; i < persons.Count; i++)
        {
            if (personID == persons[i].id)
            {
                ID = i;
                person = persons[i];
                break;
            }
        }
        Debug.Log("Игрок " + person.nickname + " отключился от игры");
        if (gamePlayManager.idPlayingPerson >= ID)
        {
            gamePlayManager.idPlayingPerson--;
        }
        persons.Remove(person);
        scoreTableManager.updateTable();
    }


    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {


        bool isKicked = otherPlayer.ActorNumber - 1 == persons[gamePlayManager.idPlayingPerson].id;

        disconnectPerson(otherPlayer.ActorNumber - 1);
        if (isKicked)
        {
            gamePlayManager.SelectNextPersonToPlay();
            //gamePlayManager.PV.RPC("SelectNextPersonToPlayOnButtonClick", RpcTarget.Others);
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
        base.OnLeftRoom();
    }

    public void LeaveLobbyFunc()
    {
        PhotonNetwork.LeaveRoom();
    }

    void Awake()
    {
        persons = new List<Person>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
