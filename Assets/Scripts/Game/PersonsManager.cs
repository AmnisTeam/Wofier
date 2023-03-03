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
        Debug.Log("����� " + person.nickname + " ����������� � ����");
    }

    public void disconnectPerson(int personID)
    {
        Person person = null;
        for (int i = 0; i < persons.Count; i++)
        {
            if (personID == persons[i].id)
            {
                person = persons[i];
                break;
            }
        }
        Debug.Log(person.nickname);
        //if (person == null)
        //{
            Debug.Log("����� " + person.nickname + " ���������� �� ����");
            /*        if (gamePlayManager.idPlayingPerson >= personID)
                    {
                        gamePlayManager.idPlayingPerson--;
                    }*/
            persons.Remove(person);
            scoreTableManager.updateTable();
        //}
    }


    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (otherPlayer.ActorNumber - 1 == persons[gamePlayManager.idPlayingPerson].id)
        {
            gamePlayManager.PV.RPC("SelectNextPersonToPlayOnButtonClick", RpcTarget.All);
        }
        disconnectPerson(otherPlayer.ActorNumber - 1);
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
