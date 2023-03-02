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

    public GameObject letterPrifab;
    public int countStartLetters = 5;
    public bool isDrag = false;
    public int dragSlotId = -1;

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
        //IntersectSlots();
    }
}
