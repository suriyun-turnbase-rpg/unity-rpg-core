using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIRaidEvent : UIDataItem<RaidEvent>
{
    [Header("General")]
    public Text textTitle;
    public Text textDescription;
    public Image imageIcon;
    public Text textRecommendBattlePoint;
    public UIStamina uiRequireStamina;
    public Text textHpPerMaxHp;
    public Text textHpPercent;
    public Image imageHpGage;
    public Text textStartTime;
    public Text textEndTime;

    public override void Clear()
    {
        // Don't clear
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(RaidEvent data)
    {
        if (textTitle != null)
            textTitle.text = data.StageData.Title;

        if (textDescription != null)
            textDescription.text = data.StageData.Description;

        if (imageIcon != null)
            imageIcon.sprite = data == null ? null : data.StageData.icon;

        if (textRecommendBattlePoint != null)
            textRecommendBattlePoint.text = data.StageData.recommendBattlePoint.ToString("N0");

        if (uiRequireStamina != null)
        {
            var staminaData = PlayerStamina.StageStamina.Clone();
            if (!string.IsNullOrEmpty(data.StageData.requireCustomStamina) && PlayerStamina.HasStamina(data.StageData.requireCustomStamina))
                staminaData = PlayerStamina.GetStamina(data.StageData.requireCustomStamina).Clone();
            staminaData.SetAmount(data.StageData.requireStamina, 0);
            uiRequireStamina.SetData(staminaData);
        }

        var maxHp = data.StageData.GetCharacter().Attributes.hp;
        var rate = (float)data.RemainingHp / (float)maxHp;

        if (textHpPerMaxHp != null)
            textHpPerMaxHp.text = data.RemainingHp.ToString("N0") + "/" + maxHp.ToString("N0");

        if (textHpPercent != null)
            textHpPercent.text = (rate * 100).ToString("N2") + "%";

        if (imageHpGage != null)
            imageHpGage.fillAmount = rate;

        if (textStartTime != null)
        {
            var d = new System.DateTime(1970, 1, 1);
            d = d.AddSeconds(data.startTime);
            textStartTime.text = d.GetPrettyDate();
        }

        if (textEndTime != null)
        {
            var d = new System.DateTime(1970, 1, 1);
            d = d.AddSeconds(data.endTime);
            textEndTime.text = d.GetPrettyDate();
        }
    }
}
