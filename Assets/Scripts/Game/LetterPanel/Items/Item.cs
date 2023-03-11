using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IIdexable, IDestroyable
{
    public GameObject draggedObjectPrifab;
    public bool canDragging = true;
    public Color colorOnDrag;
    public float sizeOnDrag = 0.8f;
    public int idInInventory;
    public int type_id;

    public float timeToDestroyAnimation = 0.2f;

    public Inventory inventory;

    GameObject soundGameObject;
    SoundEffects soundEffects;

    public virtual void ItemStart()
    {
        soundGameObject = GameObject.FindWithTag("SOUND_EFFECTS_TAG");
        soundEffects = soundGameObject.GetComponent<SoundEffects>();
    }

    public virtual void ItemUpdate()
    {
    }

    public virtual bool OnSetItem(Tile tile, Person person)
    {
        Destroy(gameObject);
        return true;
    }

    public void ConstructorItem(Inventory inventory, int idInInventory)
    {
        this.inventory = inventory;
        this.idInInventory = idInInventory;
    }

    public GameObject GetTile()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if(hit)
            if (hit.collider.gameObject.GetComponent<Tile>())
                return hit.collider.gameObject;
        return null;
    }

    public void MoveItemToSlot(int id)
    {
        if(idInInventory >= 0)
            inventory.items[idInInventory] = null;

        idInInventory = id >= 0 ? id : inventory.GetLastFreeSlotId();
        transform.SetParent(inventory.slots[idInInventory].transform);
        inventory.items[idInInventory] = gameObject;
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }

    public abstract GameObject GetDraggedObject();

    public void OnBeginDrag(PointerEventData eventData)
    {
        MouseObject.Drag(gameObject);
        Camera.main.GetComponent<MoveOnMapCamera>().workDetector.AddLoker("drag_item");
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        transform.localScale = new Vector3(sizeOnDrag, sizeOnDrag, 1);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MouseObject.EndDrag();
        
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<Image>().raycastTarget = true;
        Camera.main.GetComponent<MoveOnMapCamera>().workDetector.RemoveLocker("drag_item");
        Debug.Log(inventory.gamePlayManager.me.nickname);
        Debug.Log(inventory.gamePlayManager.idPlayingPerson);
        if (inventory.gamePlayManager.me.id == inventory.gamePlayManager.personManager.persons[inventory.gamePlayManager.idPlayingPerson].id)
        //if (inventory.gamePlayManager.personManager.persons[PhotonNetwork.LocalPlayer.ActorNumber - 1].id == inventory.gamePlayManager.personManager.persons[inventory.gamePlayManager.idPlayingPerson].id)
        {
            GameObject tile = GetTile();
            if (tile && tile.GetComponent<Tile>().isCanSetItem)
            {
                try
                {
                    soundEffects.PlaySound("tile_set");
                }
                catch
                {
                    Console.WriteLine("Perhaps forgot to initialize Item Start in the class heir");
                    throw new Exception("Perhaps forgot to initialize Item Start in the class heir");
                }

                bool settled = OnSetItem(tile.GetComponent<Tile>(), inventory.gamePlayManager.me);
                if (settled)
                {
                    if (idInInventory >= 0)
                        inventory.items[idInInventory] = null;
                    OnDestroyObject();
                }
                else
                {
                    soundEffects.PlaySound("tile_error");
                    MoveItemToSlot(idInInventory);
                }
            }
            else
            {
                soundEffects.PlaySound("tile_error");
                MoveItemToSlot(idInInventory);
            }
        }
        else
        {
            soundEffects.PlaySound("tile_error");
            MoveItemToSlot(idInInventory);
        }
    }

    public int GetId()
    {
        return type_id;
    }

    public void SetId(int id)
    {
        type_id = id;
    }
    public void OnDestroyObject()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<Image>().raycastTarget = true;
        GameObject item = gameObject;
        LeanTween.value(1, 0, timeToDestroyAnimation).setEaseInOutCubic().setOnUpdate((float value) =>
        {
            if(item)
                transform.localScale = new Vector2(value, value);
        }).setOnComplete(() => {
            if(item)
                Destroy(gameObject);
        });
    }
}
