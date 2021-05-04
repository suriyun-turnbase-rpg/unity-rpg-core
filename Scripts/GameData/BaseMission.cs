using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BaseMission : BaseGameData
{
    [Header("Generic Info")]
    public StageType stageType;
    public Sprite icon;
    public int recommendBattlePoint;
    [Header("Stamina")]
    public int requireStamina;
    [Tooltip("If this is not empty it will use stamina which its ID is this value")]
    public string requireCustomStamina;
    [Header("Rewards")]
    public CurrencyRandomAmount[] randomCustomCurrencies;
    public int randomSoftCurrencyMinAmount;
    public int randomSoftCurrencyMaxAmount;
    public int rewardPlayerExp;
    public int rewardClanExp;
    public int rewardCharacterExp;
    public ItemDrop[] rewardItems;
    [Header("First Clear Rewards")]
    public CurrencyAmount[] firstClearRewardCustomCurrencies;
    public int firstClearRewardSoftCurrency;
    public int firstClearRewardHardCurrency;
    public int firstClearRewardPlayerExp;
    public ItemAmount[] firstClearRewardItems;
    [Header("Event")]
    public StageAvailability[] availabilities;
    public bool hasAvailableDate;
    [Range(1, 9999)]
    public int startYear = 1;
    public Month startMonth = Month.January;
    [Range(1, 31)]
    public int startDay = 1;
    public int durationDays;

    protected override void OnValidate()
    {
        base.OnValidate();
        bool hasChanges = false;
        bool entryHasChanges = false;
        if (availabilities != null)
        {
            for (int i = 0; i < availabilities.Length; ++i)
            {
                availabilities[i] = availabilities[i].ValidateSetting(out entryHasChanges);
                hasChanges = hasChanges || entryHasChanges;
            }
        }
        int daysInMonth = System.DateTime.DaysInMonth(startYear, (int)startMonth);
        if (startDay > daysInMonth)
        {
            startDay = daysInMonth;
            hasChanges = true;
        }
#if UNITY_EDITOR
        if (hasChanges)
            EditorUtility.SetDirty(this);
#endif
    }

    public virtual List<PlayerItem> GetRewardItems()
    {
        var dict = new Dictionary<string, PlayerItem>();
        foreach (var rewardItem in rewardItems)
        {
            var item = rewardItem.item;
            var newEntry = new PlayerItem();
            newEntry.Id = item.Id;
            newEntry.DataId = item.Id;
            newEntry.Amount = 1;
            dict[item.Id] = newEntry;
        }
        return new List<PlayerItem>(dict.Values);
    }

    public virtual List<PlayerItem> GetFirstClearRewardItems()
    {
        var dict = new Dictionary<string, PlayerItem>();
        foreach (var rewardItem in firstClearRewardItems)
        {
            var item = rewardItem.item;
            var newEntry = new PlayerItem();
            newEntry.Id = item.Id;
            newEntry.DataId = item.Id;
            newEntry.Amount = 1;
            dict[item.Id] = newEntry;
        }
        return new List<PlayerItem>(dict.Values);
    }
}
