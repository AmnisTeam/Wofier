using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    public GameObject[] items;
    public MapGenerator mapGenerator;
    public GamePlayManager gamePlayManager;
    public Camera cam;
    public GameObject swapButton;

    public GameObject letterPrifab;
    public int countStartLetters = 5;
    public bool isDrag = false;
    public int dragSlotId = -1;
    public bool isSwap = false;

    public float timeToAppearanceSwapButton;
    public float timeToAppearanceShufleButton;

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    //private string alphabet = "HELLOHELLOHELLOHELLO";

    public int GetLastFreeSlotId()
    {
        for(int x = 0; x < slots.Length; x++)
        {
            if (!items[x])
                return x;
        }
        return -1;
    }
    public void AddRandomLetters(int count)
    {
        if (count > slots.Length)
            count = slots.Length;
        for(int x = 0; x < count; x++)
        {
            int id = GetLastFreeSlotId();
            if (id != -1)
            {
                GameObject letter = Instantiate(letterPrifab, slots[id].transform);
                letter.GetComponent<LetterItem>().letter = alphabet[Random.Range(0, alphabet.Length - 1)];
                //letter.GetComponent<LetterItem>().letter = alphabet[id];
                letter.GetComponent<LetterItem>().ConstructorItem(this, id);
                items[id] = letter;
            }
        }
    }

    public void ResetLetters()
    {
        isSwap = true;
        gamePlayManager.PV.RPC("SelectNextPersonToPlayOnButtonClick", RpcTarget.All);
        //gamePlayManager.SetTimerToPlayerOnePerson(0f);
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                Destroy(items[i]);
            items[i] = null;
        }

        AddRandomLetters(countStartLetters);
    }

    void Awake()
    {
        items = new GameObject[slots.Length];
    }

    void Start()
    {
        AddRandomLetters(countStartLetters);
    }

    void Update()
    {
        if (gamePlayManager.personManager.persons[gamePlayManager.idPlayingPerson].id == gamePlayManager.me.id)
        {
            swapButton.SetActive(true);
            swapButton.GetComponent<CanvasGroup>().LeanAlpha(1, timeToAppearanceSwapButton);
        }
        else
        {
            swapButton.GetComponent<CanvasGroup>().LeanAlpha(0, timeToAppearanceSwapButton);
            swapButton.SetActive(false);
            gamePlayManager.acceptWordButton.SetActive(false);
            gamePlayManager.acceptWordButton.GetComponent<CanvasGroup>().LeanAlpha(0, gamePlayManager.timeToAppearanceAcceptWordButton);
        }
    }
}
