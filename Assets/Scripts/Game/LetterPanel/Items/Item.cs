using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject draggedObjectPrifab;
    public bool canDragging = true;
    public Color colorOnDrag;
    public float sizeOnDrag = 0.8f;
    public int idInInventory;

    public Inventory inventory;

    void Start()
    {
        
    }

    void Update()
    {

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
        MouseObject.draggedObject = gameObject;
        MouseObject.isDrag = true;
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
        MouseObject.draggedObject = null;
        MouseObject.isDrag = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<Image>().raycastTarget = true;
        if (inventory.gamePlayManager.me.id == inventory.gamePlayManager.personManager.persons[inventory.gamePlayManager.idPlayingPerson].id)
        {
            GameObject tile = GetTile();
            if (tile && tile.GetComponent<Tile>().isCanSetItem)
            {
                tile.GetComponent<Tile>().OnSetItem(this, inventory.gamePlayManager.me);
                if(idInInventory >= 0)
                    inventory.items[idInInventory] = null;
                Destroy(gameObject);
            }
            else
            {
                MoveItemToSlot(idInInventory);
            }
        }
        else
        {
            MoveItemToSlot(idInInventory);
        }
    }
}
