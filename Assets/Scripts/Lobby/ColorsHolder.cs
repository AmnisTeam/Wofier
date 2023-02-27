using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Color.magenta,
            Color.cyan
        };
        
        freeColorsIdx = new List<int>();
            
        for (int i = 0; i < colors.Count; i++)
            freeColorsIdx.Add(i);
        
    }
    /*
    public Color32 GetRandomColor()
    {
        if (colors.Count > 0)
        {
            var rnd = new System.Random();
            var n = rnd.Next(0, colors.Count);

            Color32 savedColor = colors[n];
            colors.RemoveAt(n);

            return savedColor;
        }
        else { return Color.black; }
    }

    public void AddColor(Color32 color)
    {
        colors.Add(color);
    }*/

    /*
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
    }*/

    public int getRandomIndex()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(0, freeColorsIdx.Count);

        return index;
    }

    public void refillFreeIndicies()
    {
        freeColorsIdx = new List<int>(Enumerable.Range(0, colors.Count).ToArray());
    }

    public void putColorBack(Color32 color)
    {
        colors.Add(color);
    }

}
