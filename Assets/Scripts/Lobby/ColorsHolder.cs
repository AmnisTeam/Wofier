using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorsHolder : MonoBehaviour
{
    public static ColorsHolder instance;
    public List<Color32> colors;
    public List<int> freeColorsIdx;

    public ColorsHolder()
    {
        instance = this;
    }

    void Start()
    {
        colors = new List<Color32>
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.yellow,
            Color.magenta
        };
        freeColorsIdx = new List<int>();
            
        for (int i = 0; i < colors.Count; i++)
            freeColorsIdx.Add(i);
    }

    //public Color32 GetRandomColor()
    //{
    //    if (colors.Count > 0)
    //    {
    //        var rnd = new System.Random();
    //        var n = rnd.Next(0, colors.Count);

    //        Color32 savedColor = colors[n];
    //        colors.RemoveAt(n);

    //        return savedColor; 
    //    }
    //    else { return Color.white; }
    //}

    //public void AddColor(Color32 color)
    //{
    //    colors.Add(color);
    //}

    public int GetRandomIdx()
    {
        if (freeColorsIdx.Count > 0)
        {
            var rnd = new System.Random();
            var n = rnd.Next(0, colors.Count);

            int savedColorIdx = freeColorsIdx[n];
            freeColorsIdx.RemoveAt(n);

            return savedColorIdx;
        }
        else { return -1; }
    }

    public void AddColor(int colorIdx)
    {
        freeColorsIdx.Add(colorIdx);
    }

}
