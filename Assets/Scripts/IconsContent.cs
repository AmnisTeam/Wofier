using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconsContent : MonoBehaviour
{
    public static IconsContent instance;
    public Sprite[] icons;

    public IconsContent()
    {
        instance = this;
    }
}
