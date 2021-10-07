using UnityEngine;
using UnityEngine.UI;

public class UIBossStats : MonoBehaviour
{
    public Text textTitle;
    public Image imageIcon;
    public GameObject characterStatsRoot;
    public Text textHpPerMaxHp;
    public Text textHpPercent;
    public Image imageHpGage;
    public UILevel uiLevel;
    public UICharacterBuff[] uiBuffs;

    private void Awake()
    {
        if (characterStatsRoot != null)
            characterStatsRoot.SetActive(false);
    }

    private void Update()
    {
        var character = BaseGamePlayManager.Singleton.GetBossCharacterEntity();

        if (characterStatsRoot != null)
            characterStatsRoot.SetActive(character != null);

        if (character != null)
        {
            var itemData = character.Item.ItemData;
            var rate = (float)character.Hp / (float)character.MaxHp;

            if (textHpPerMaxHp != null)
                textHpPerMaxHp.text = character.Hp.ToString("N0") + "/" + character.MaxHp.ToString("N0");

            if (textHpPercent != null)
                textHpPercent.text = (rate * 100).ToString("N2") + "%";

            if (imageHpGage != null)
                imageHpGage.fillAmount = rate;

            if (textTitle != null)
                textTitle.text = itemData.Title;

            if (imageIcon != null)
                imageIcon.sprite = itemData.icon;

            if (uiLevel != null)
            {
                uiLevel.level = character.Item.Level;
                uiLevel.maxLevel = character.Item.MaxLevel;
                uiLevel.currentExp = character.Item.CurrentExp;
                uiLevel.requiredExp = character.Item.RequiredExp;
            }

            var i = 0;
            var buffKeys = character.Buffs.Keys;
            foreach (var buffKey in buffKeys)
            {
                if (i >= uiBuffs.Length)
                    break;
                var ui = uiBuffs[i];
                ui.buff = character.Buffs[buffKey];
                ui.Show();
                ++i;
            }
            for (; i < uiBuffs.Length; ++i)
            {
                var ui = uiBuffs[i];
                ui.Hide();
            }
        }
    }
}
