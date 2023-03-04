using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMath
{
    public static int SelectRandomElement(float[] notNormalProbabilities)
    {
        float m = 0;
        for (int x = 0; x < notNormalProbabilities.Length; x++)
            m += notNormalProbabilities[x];

        float[] probabilities = new float[notNormalProbabilities.Length];
        for (int x = 0; x < notNormalProbabilities.Length; x++)
            probabilities[x] = notNormalProbabilities[x] / m;

        Vector2[] points = new Vector2[probabilities.Length];

        float F = 0;
        for (int x = 0; x < probabilities.Length; x++)
        {
            F += probabilities[x];
            float posX = x;

            points[x] = new Vector2(F, posX);
        }

        float randomValue = UnityEngine.Random.Range(0, 0.9999f);
        int id = 0;
        for (int x = 0; x < probabilities.Length; x++)
        {
            float leftPoint = x > 0 ? points[x - 1].x : 0;
            if (randomValue >= leftPoint && randomValue < points[x].x)
            {
                id = (int)(points[x].y);
                break;
            }
        }

        return id;
    }

}
