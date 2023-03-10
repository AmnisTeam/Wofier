using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragInventoryItems : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int idSlot;
    private bool mouseOver = false;
    GameObject soundGameObject;
    SoundEffects soundEffects;

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
            Camera.main.GetComponent<MoveOnMapCamera>().workDetector.RemoveLocker("drag_item");
            if (!item.inventory.items[idSlot])
                item.MoveItemToSlot(idSlot);
            else
                item.MoveItemToSlot(item.idInInventory);

            MouseObject.EndDrag();
        }
    }

    void Start()
    {
        soundGameObject = GameObject.FindWithTag("SOUND_EFFECTS_TAG");
        soundEffects = soundGameObject.GetComponent<SoundEffects>();
    }

    void Update()
    {
        if (mouseOver)
            OnPointerOver();
    }
}
