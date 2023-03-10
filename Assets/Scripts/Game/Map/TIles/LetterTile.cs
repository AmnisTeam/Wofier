using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Photon.Pun.Demo.Shared.DocLinks;

public class LetterTile : Tile
{
    public GameObject letterObject;
    public TMPro.TMP_Text letterText;
    public TMPro.TMP_Text priceLeftDown;
    public TMPro.TMP_Text priceRightTop;
    public char letter;
    private char oldLetter;

    public bool isHaveLetter = false;
    private bool isHaveLetterOld = false;

    public bool inWord = false;
    private bool oldInWord;

    public Color colorWithoutLetter;
    public Color colorWithLetter;
    public Color colorInWord;

    public float priceFactor = 1;

    public PhotonView PV;

    public int _isDrag = 0;

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

    public float GetLetterPrice()
    {
        return LetterItem.GetLetterPrice(letter, priceFactor);
    }

    public bool isPlayerInGame(int playerID)
    {
        bool isHave = false;
        for (int i = 0; i < inventory.gamePlayManager.personManager.persons.Count; i++)
        {
            if (playerID == inventory.gamePlayManager.personManager.persons[i].id)
            {
                isHave = true;
                break;
            }
        }
        return isHave;
    }

    public virtual void OnHaveLetter(bool isHaveLetter)
    {
        if(isHaveLetter)
        {
            letterObject.gameObject.SetActive(true);
            GetComponent<SpriteRenderer>().color = inWord ? colorInWord : colorWithLetter;

            if (GetLetterPrice() != 0)
            {
                priceLeftDown.text = GetLetterPrice().ToString();
                priceRightTop.text = GetLetterPrice().ToString();
            }
            else
            {
                priceLeftDown.text = "";
                priceRightTop.text = "";
            }
    }
        else
        {
            letterObject.gameObject.SetActive(false);
            GetComponent<SpriteRenderer>().color = colorWithoutLetter;
        }
    }

    public virtual int CanCompleteWord(TileWord word)
    {
        if (inventory.gamePlayManager.numberOfPlayerStep == 0)
            return -1;
        return 1;
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
/*#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                OnTouchDown();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                OnTouchUp();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
                MouseObject.isDrag = true;
            else
                MouseObject.isDrag = false;
        }
#endif*/


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
            OnHaveLetter(isHaveLetter);
        }

        if (!inWord)
            if (isHaveLetter)
                if (person.id == inventory.gamePlayManager.me.id)
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
                        }
                        RPC_Request(false, person);

                        person = null;
                        isHaveLetter = false;
                    }
        if (person != null)
        {
            if (!isPlayerInGame(person.id) && !inWord)
            {
                person = null;
                isHaveLetter = false;
                RPC_Request(false, person);
            }
        }

        if(_isDrag == 2)
        {
            if(MouseObject.draggedObject)
                MouseObject.draggedObject.GetComponent<LetterItem>().OnEndDrag(null);
            MouseObject.EndDrag();
            _isDrag = 0;
        }
    }








/*    void OnTouchDown()
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
    void OnDrag()
    {
        if (MouseObject.isDrag)
        {
            MouseObject.draggedObject.GetComponent<LetterItem>().OnDrag(null);
        }
    }

    void OnTouchUp()
    {
        if (MouseObject.isDrag)
        {
            _isDrag = 2;
        }
    }*/



//#if !UNITY_ANDROID
    void OnMouseDown()
    {
        if (!inWord)
            if (isHaveLetter && person.id == inventory.gamePlayManager.me.id && !MouseObject.isDrag)
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
            //if(MouseObject.draggedObject)
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
//#endif










    public void RPC_Request(bool isSet, Person person)
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
