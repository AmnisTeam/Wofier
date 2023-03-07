using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragInventoryItems : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int idSlot;
    private bool mouseOver = false;

    public void OnDrop(PointerEventData eventData)
    {
        Item item = eventData.pointerDrag.GetComponent<Item>();
        if(!item.inventory.items[idSlot])
            item.MoveItemToSlot(idSlot);
        else
            item.MoveItemToSlot(item.idInInventory);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    public void OnPointerOver()
    {
        if(MouseObject.isDrag && !Input.GetMouseButton(0)) /*|| Input.GetTouch(0).phase != TouchPhase.Stationary)*/
        {
            Item item = MouseObject.draggedObject.GetComponent<Item>();
            item.GetComponent<CanvasGroup>().blocksRaycasts = true;
            item.GetComponent<Image>().raycastTarget = true;
            if (!item.inventory.items[idSlot])
                item.MoveItemToSlot(idSlot);
            else
                item.MoveItemToSlot(item.idInInventory);

            MouseObject.EndDrag();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (mouseOver)
            OnPointerOver();
    }
}
