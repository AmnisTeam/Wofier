using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] slots;
    public GameObject[] items;
    public MapGenerator mapGenerator;
    public Camera cam;

    public GameObject letterPrifab;
    public int countStartLetters = 5;
    public bool isDrag = false;
    public int dragSlotId = -1;

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    //public bool Intersect(Vector2 position, Rect field)
    //{
    //    if (position.x >= field.position.x && position.x <= field.position.x + field.size.x)
    //        if (position.y >= field.position.y && position.y <= field.position.y + field.size.y)
    //            return true;
    //    return false;
    //}

    //public bool IntersectMouseUI(GameObject UIElement)
    //{
    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 size = UIElement.GetComponent<RectTransform>().rect.size / 100f;
    //    Rect field = new Rect(UIElement.transform.position - new Vector3(size.x / 2, size.y / 2), size);
    //    return Intersect(mousePosition, field);
    //}
    
    //public void IntersectSlots()
    //{
    //    if (!isDrag)
    //    {
    //        for (int x = 0; x < slots.Length; x++)
    //        {
    //            if (IntersectMouseUI(slots[x]))
    //            {
    //                if (Input.GetMouseButtonDown(0))
    //                {
    //                    isDrag = true;
    //                    dragSlotId = x;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if(Input.GetMouseButtonUp(0))
    //        {
    //            dragSlotId = -1;
    //            isDrag = false;
    //        }
    //    }
        
    //    for(int x = 0; x < slots.Length; x++)
    //    {
    //        if (IntersectMouseUI(slots[x]))
    //        {
    //            slots[x].GetComponent<Image>().color = new Color(255, 0, 0, 0.5f);
    //        }
    //        else
    //        {
    //            slots[x].GetComponent<Image>().color = new Color(255, 0, 0, 0);
    //        }
    //    }
    //}

    public void AddStartItems(int count)
    {
        if (count > slots.Length)
            count = slots.Length;

        for(int x = 0; x < count; x++)
        {
            GameObject letter = Instantiate(letterPrifab, slots[x].transform);
            letter.GetComponent<LetterItem>().letter = alphabet[Random.Range(0, alphabet.Length - 1)];
            letter.GetComponent<LetterItem>().idInInventory = x;
            letter.GetComponent<LetterItem>().inventory = this;
            letter.GetComponent<LetterItem>().mapGenerator = mapGenerator;
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
