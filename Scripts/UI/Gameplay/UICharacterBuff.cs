using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterBuff : UIBase
{
    public Text textRemainsTurns;
    public Image imageRemainsTurnsGage;
    public Image imageIcon;
    public BaseCharacterBuff buff;

    private void Update()
    {
        if (buff == null)
            return;

        var rate = buff.GetCoolDownRate();

        if (imageIcon != null)
            imageIcon.sprite = buff.Buff.icon;

        if (textRemainsTurns != null)
            textRemainsTurns.text = buff.GetCoolDown() <= 0 ? "" : buff.GetCoolDown().ToString("N2");

        if (imageRemainsTurnsGage != null)
            imageRemainsTurnsGage.fillAmount = rate;
    }
}
