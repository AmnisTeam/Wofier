using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image background;

    public float scaleOnIdle;
    public Color colorOnIdle;

    public float scaleOnDown;
    public Color colorOnDown;

    public float timeOnDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.LeanScale(new Vector3(scaleOnDown, scaleOnDown, scaleOnDown), timeOnDown).setEaseInOutCubic();
        LeanTween.value(0, 1, timeOnDown).setOnUpdate((float value) =>
        {
            if(background)
                background.color = colorOnIdle + (colorOnDown - colorOnIdle) * value;
        }).setEaseInOutCubic();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.LeanScale(new Vector3(scaleOnIdle, scaleOnIdle, scaleOnIdle), timeOnDown).setEaseInOutCubic();
        LTDescr colorLT = LeanTween.value(1, 0, timeOnDown).setOnUpdate((float value) =>
        {
            if (background)
                background.color = colorOnIdle + (colorOnDown - colorOnIdle) * value;
        }).setEaseInOutCubic();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
