using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIFollowWorldObject))]
public class UICharacterStatsGeneric : UIBase
{
    public Text textTitle;
    public Image imageIcon;
    public Image imageIcon2;
    public Image imageIcon3;
    public GameObject characterStatsRoot;
    public Text textHpPerMaxHp;
    public Text textHpPercent;
    public Image imageHpGage;
    public UILevel uiLevel;
    public UICharacterBuff[] uiBuffs;
    public BaseCharacterEntity character;
    public bool notFollowCharacter;
    public bool hideIfCharacterIsBoss;

    public UIFollowWorldObject CacheObjectFollower { get; private set; }

    private Canvas attachedCanvas;
    private bool attachedWorldSpaceCanvas;

    protected override void Awake()
    {
        base.Awake();
        if (!characterStatsRoot)
            characterStatsRoot = root;
        CacheObjectFollower = GetComponent<UIFollowWorldObject>();
        attachedCanvas = GetComponent<Canvas>();
        if (attachedCanvas != null)
            attachedWorldSpaceCanvas = (attachedCanvas.renderMode == RenderMode.WorldSpace);
    }

    private void Start()
    {
        if (notFollowCharacter || attachedWorldSpaceCanvas)
        {
            CacheObjectFollower.enabled = false;
            CacheObjectFollower.CachePositionFollower.enabled = false;
        }
    }

    public void Attach(Transform uiContainer, BaseCharacterEntity character)
    {
        this.character = character;
        if (attachedWorldSpaceCanvas)
        {
            transform.SetParent(character.uiContainer, true);
        }
        else
        {
            transform.SetParent(uiContainer);
            transform.localScale = Vector3.one;
        }
        transform.localPosition = Vector3.zero;
    }

    protected virtual void Update()
    {
        if ((hideIfCharacterIsBoss && character.IsBoss) || !character)
        {
            HideCharacterStats();
            return;
        }

        if (!attachedWorldSpaceCanvas && !notFollowCharacter)
            CacheObjectFollower.targetObject = character.uiContainer;

        var hp = (float)character.Hp;
        var maxHp = (float)character.MaxHp;
        var itemData = character.Item.ItemData;
        var rate = hp / maxHp;

        if (textHpPerMaxHp != null)
            textHpPerMaxHp.text = hp.ToString("N0") + "/" + maxHp.ToString("N0");

        if (textHpPercent != null)
            textHpPercent.text = (rate * 100).ToString("N2") + "%";

        if (imageHpGage != null)
            imageHpGage.fillAmount = rate;

        if (textTitle != null)
            textTitle.text = itemData.Title;

        if (imageIcon != null)
        {
            imageIcon.sprite = itemData.icon;
            imageIcon.preserveAspect = true;
        }

        if (imageIcon2 != null)
        {
            imageIcon2.sprite = itemData.icon2;
            imageIcon2.preserveAspect = true;
        }

        if (imageIcon3 != null)
        {
            imageIcon3.sprite = itemData.icon3;
            imageIcon3.preserveAspect = true;
        }

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

    public void ShowCharacterStats()
    {
        if (hideIfCharacterIsBoss && character.IsBoss)
            return;
        characterStatsRoot.SetActive(true);
    }

    public void HideCharacterStats()
    {
        characterStatsRoot.SetActive(false);
    }
}
