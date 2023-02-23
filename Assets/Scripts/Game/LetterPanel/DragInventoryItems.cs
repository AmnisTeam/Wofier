using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragInventoryItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int idSlot;
    public Inventory inventory;
    public GameObject container;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(inventory.items[idSlot])
            inventory.items[idSlot].GetComponent<Item>().OnDrag(eventData, container);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inventory.items[idSlot])
            inventory.items[idSlot].GetComponent<Item>().OnDragging(eventData, container);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventory.items[idSlot])
            inventory.items[idSlot].GetComponent<Item>().onEndDrag(eventData, container);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
