using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterTile : Tile
{
    public TMPro.TMP_Text letterText;
    public char letter;
    private char oldLetter;

    public bool isHaveLetter = false;
    private bool isHaveLetterOld = false;

    public bool inWord = false;
    private bool oldInWord;

    public Color colorWithoutLetter;
    public Color colorWithLetter;
    public Color colorInWord;

    public PhotonView PV;

    private int _isDrag = 0;

    public void SetLetter(char letter, Person person)
    {
        this.letter = letter;
        isHaveLetter = true;
        this.person = person;
    }

    public void UnsetLetter()
    {
        isHaveLetter = false;
        person = null;
        inventory.gamePlayManager.wordChecker.TryFindWord();
    }

    public override void OnSetItem(Item item, Person person)
    {
        base.OnSetItem(item, person);
        LetterItem letterItem = item as LetterItem;
        if(letterItem)
        {
            SetLetter(letterItem.letter, person);
            inventory.gamePlayManager.wordChecker.TryFindWord();
            RPC_Request(true, person);
        }
    }

    public int GetLetterPrice()
    {
        switch(letter)
        {
            case 'A': return 1;
            case 'B': return 3;
            case 'C': return 3;
            case 'D': return 2;
            case 'E': return 1;
            case 'F': return 4;
            case 'G': return 2;
            case 'H': return 4;
            case 'I': return 1;
            case 'J': return 7;
            case 'K': return 5;
            case 'L': return 4;
            case 'M': return 3;
            case 'N': return 1;
            case 'O': return 1;
            case 'P': return 3;
            case 'Q': return 8;
            case 'R': return 1;
            case 'S': return 2;
            case 'T': return 2;
            case 'U': return 5;
            case 'V': return 4;
            case 'W': return 4;
            case 'X': return 7;
            case 'Y': return 2;
            case 'Z': return 10;
            default:  return 1;
        }
    }

    void Awake()
    {
        oldLetter = letter;
        isHaveLetterOld = isHaveLetter;
    }

    void Start()
    {

    }


    void Update()
    {
        if (oldLetter != letter)
        {
            letterText.text = letter.ToString();

            oldLetter = letter;
        }

        if (isHaveLetterOld != isHaveLetter || oldInWord != inWord)
        {
            isHaveLetterOld = isHaveLetter;
            isCanSetItem = !isHaveLetter;
            oldInWord = inWord;
            if (isHaveLetter)
            {
                letterText.gameObject.SetActive(true);
                GetComponent<SpriteRenderer>().color = inWord ? colorInWord : colorWithLetter;
            }
            else
            {
                letterText.gameObject.SetActive(false);
                GetComponent<SpriteRenderer>().color = colorWithoutLetter;
            }
        }

        if (!inWord)
            if (isHaveLetter)
                if (person == inventory.gamePlayManager.me)
                    if (inventory.gamePlayManager.me.id != inventory.gamePlayManager.personManager.persons[inventory.gamePlayManager.idPlayingPerson].id)
                    {
                        int slotId = inventory.GetLastFreeSlotId();
                        if (slotId != -1)
                        {
                            GameObject prifabItem = GameObject.Find("RegisterItems").GetComponent<RegisterGameObjects>().gameObjects[0];
                            GameObject letterItem = Instantiate(prifabItem, inventory.slots[slotId].transform);
                            letterItem.GetComponent<LetterItem>().letter = letter;
                            letterItem.GetComponent<LetterItem>().ConstructorItem(inventory, slotId);
                            inventory.items[slotId] = letterItem;
                            RPC_Request(false, person);
                        }

                        person = null;
                        isHaveLetter = false;
                    }

        if(_isDrag == 2)
        {
            if(MouseObject.isDrag)
                MouseObject.draggedObject.GetComponent<LetterItem>().OnEndDrag(null);
            MouseObject.EndDrag();
            _isDrag = 0;
        }
    }

    void OnMouseDown()
    {
        if (!inWord)
            if (isHaveLetter && person.id == inventory.gamePlayManager.me.id)
            {
                GameObject item = Instantiate(GameObject.Find("RegisterItems").GetComponent<RegisterGameObjects>().gameObjects[0], inventory.transform);
                item.GetComponent<LetterItem>().ConstructorItem(inventory, -1);
                item.GetComponent<LetterItem>().letter = letter;
                item.transform.position = transform.position;
                item.GetComponent<LetterItem>().OnBeginDrag(null);
                MouseObject.Drag(item);
                _isDrag = 1;
                UnsetLetter();
                RPC_Request(false, person);
            }
    }

    void OnMouseDrag()
    {
        if (MouseObject.isDrag)
        {
            MouseObject.draggedObject.GetComponent<LetterItem>().OnDrag(null);
        }
    }

    void OnMouseUp()
    {
        if (MouseObject.isDrag)
        {
            _isDrag = 2;
        }
    }

    private void RPC_Request(bool isSet, Person person)
    {
        Vector2 sizeTile = inventory.mapGenerator.GetSizeTile();
        Vector2 mapPos = inventory.mapGenerator.GetLeftTopMap();
        Vector2 tilePos = (transform.position.ToXY() + sizeTile / 2 - mapPos) / sizeTile;
        if (person == null)
            inventory.mapGenerator.PV.RPC("UpdateTileOnEdit", RpcTarget.Others, (int)tilePos.x, (int)tilePos.y, isSet, -1);
        else
            inventory.mapGenerator.PV.RPC("UpdateTileOnEdit", RpcTarget.Others, (int)tilePos.x, (int)tilePos.y, isSet, person.id);
    }

}
