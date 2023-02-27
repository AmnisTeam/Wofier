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

    public int GetLastFreeSlotId()
    {
        for(int x = 0; x < slots.Length; x++)
        {
            if (!items[x])
                return x;
        }
        return -1;
    }
    public void AddStartItems(int count)
    {
        if (count > slots.Length)
            count = slots.Length;
        string str = "HELLOQWER";
        for(int x = 0; x < count; x++)
        {
            GameObject letter = Instantiate(letterPrifab, slots[x].transform);
            //letter.GetComponent<LetterItem>().letter = alphabet[Random.Range(0, alphabet.Length - 1)];
            letter.GetComponent<LetterItem>().letter = str[x];
            letter.GetComponent<LetterItem>().ConstructorItem(this, x);
            items[x] = letter;
        }
    }

    void Awake()
    {
        items = new GameObject[slots.Length];
    }

    void Start()
    {
        AddStartItems(countStartLetters);
    }

    void Update()
    {
        //IntersectSlots();
    }
}
