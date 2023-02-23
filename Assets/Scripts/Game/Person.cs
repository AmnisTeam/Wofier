using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Person
{
    public int id;
    public string nickname;
    public Color color;
    public int iconId;
    public int score;

    public Person(int id, string nickname, Color color, int iconId)
    {
        this.id = id;
        this.nickname = nickname;
        this.color = color;
        this.iconId = iconId;
        this.score = 0;
    }
}
