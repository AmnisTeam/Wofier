using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public GameObject draggedObjectPrifab;
    public bool canDragging = true;
    public Color colorOnDrag;
    public float sizeOnDrag = 0.8f;
    public int idInInventory;

    public Inventory inventory;
    public MapGenerator mapGenerator;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public GameObject GetTile()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hit.collider.gameObject.GetComponent<Tile>())
            return hit.collider.gameObject;
        return null;
    }

    public abstract GameObject GetDraggedObject();

    public virtual void OnDrag(PointerEventData eventData, GameObject container)
    {
        //container.GetComponent<Image>().color = colorOnDrag;
    }
    public virtual void OnDragging(PointerEventData eventData, GameObject container)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
        transform.localScale = new Vector3(sizeOnDrag, sizeOnDrag, 1);

    }

    public virtual void onEndDrag(PointerEventData eventData, GameObject container)
    {
        //container.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        transform.position = inventory.slots[idInInventory].transform.position;
        transform.localScale = new Vector3(1, 1, 1);

        GameObject tileObject = GetTile();
        if (tileObject)
            tileObject.GetComponent<Tile>().OnSetItem(this);
        inventory.items[idInInventory] = null;
        Destroy(gameObject);
    }
}
