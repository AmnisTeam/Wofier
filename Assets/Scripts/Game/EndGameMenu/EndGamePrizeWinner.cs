using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePrizeWinner : MonoBehaviour
{
    public Image icon;
    public TMPro.TMP_Text nickname;
    public TMPro.TMP_Text points;

    private IconsContent iconsContent;

    public void SetWinner(Sprite iconSprite, Color iconColor, string nickname, int points)
    {
        icon.sprite = iconSprite;
        icon.color = iconColor;
        this.nickname.text = nickname;
        this.points.text = points.ToString() + " points";
    }

    public void SetWinner(Person person)
    {
        SetWinner(iconsContent.icons[person.iconId],
                  person.color,
                  person.nickname,
                  (int)person.score);
    }

    private void Awake()
    {
        iconsContent = GameObject.FindWithTag("ICONS_CONTENT_TAG").GetComponent<IconsContent>();
    }
}
