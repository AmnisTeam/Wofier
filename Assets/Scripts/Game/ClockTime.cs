using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTime : MonoBehaviour
{
    public TMPro.TMP_Text clockText;
    public float time;

    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime;

        int secundes = (int)(time % 60);
        int minutes = (int)(time / 60);

        clockText.text = minutes.ToString("00") + ":" + secundes.ToString("00");
    }
}
