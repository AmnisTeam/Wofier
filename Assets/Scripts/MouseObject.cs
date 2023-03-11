using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObject
{
    public static bool isDrag = false;
    public static GameObject draggedObject;

    public static void Drag(GameObject draggedObject)
    {
        isDrag = true;
        //if (MouseObject.draggedObject)
        //{
        //    IDestroyable destroyable = draggedObject.GetComponent<IDestroyable>();
        //    if (destroyable != null)
        //        destroyable.OnDestroyObject();
        //    else
        //        GameObject.Destroy(draggedObject);
        //}
        MouseObject.draggedObject = draggedObject;
    }

    public static void EndDrag()
    {
        isDrag = false;
        draggedObject = null;
    }
}
