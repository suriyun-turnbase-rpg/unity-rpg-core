using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BaseStage : BaseGameData
{
    public string stageNumber;
    public Sprite icon;
    public int recommendBattlePoint;
    [Header("Stamina")]
    public int requireStamina;
    [Tooltip("If this is not empty it will use stamina which its ID is this value")]
    public string requireCustomStamina;
    [Header("Rewards")]
    public int randomSoftCurrencyMinAmount;
    public int randomSoftCurrencyMaxAmount;
    public int rewardPlayerExp;
    public int rewardCharacterExp;
    public ItemDrop[] rewardItems;
    [Header("First Clear Rewards")]
    public int firstClearRewardSoftCurrency;
    public int firstClearRewardHardCurrency;
    public int firstClearRewardPlayerExp;
    public ItemAmount[] firstClearRewardItems;
    [Header("Unlock")]
    public BaseStage[] unlockStages;
    [Header("Event")]
    public bool isEvent;
    public EventAvailability[] eventAvailabilities;

    protected override void OnValidate()
    {
        base.OnValidate();
        bool hasChanges = false;
        bool entryHasChanges = false;
        for (int i = 0; i < eventAvailabilities.Length; ++i)
        {
            eventAvailabilities[i] = eventAvailabilities[i].ValidateSetting(out entryHasChanges);
            hasChanges = hasChanges || entryHasChanges;
        }
#if UNITY_EDITOR
        if (hasChanges)
            EditorUtility.SetDirty(this);
#endif
    }

    public virtual List<PlayerItem> GetCharacters()
    {
        return new List<PlayerItem>();
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

    public virtual string ToJson()
    {
        // Reward Items
        var jsonRewardItems = "";
        foreach (var entry in rewardItems)
        {
            if (!string.IsNullOrEmpty(jsonRewardItems))
                jsonRewardItems += ",";
            jsonRewardItems += entry.ToJson();
        }
        jsonRewardItems = "[" + jsonRewardItems + "]";
        // First Clear Reward Items
        var jsonFirstClearRewardItems = "";
        foreach (var entry in firstClearRewardItems)
        {
            if (!string.IsNullOrEmpty(jsonFirstClearRewardItems))
                jsonFirstClearRewardItems += ",";
            jsonFirstClearRewardItems += entry.ToJson();
        }
        jsonFirstClearRewardItems = "[" + jsonFirstClearRewardItems + "]";
        // Unlock Stages
        var jsonUnlockStages = "";
        foreach (var entry in unlockStages)
        {
            if (!string.IsNullOrEmpty(jsonUnlockStages))
                jsonUnlockStages += ",";
            jsonUnlockStages += "\"" + entry.Id + "\"";
        }
        jsonUnlockStages = "[" + jsonUnlockStages + "]";
        return "{\"id\":\"" + Id + "\"," +
            "\"recommendBattlePoint\":" + recommendBattlePoint + "," +
            "\"requireStamina\":" + requireStamina + "," +
            "\"requireCustomStamina\":\"" + requireCustomStamina + "\"," +
            "\"randomSoftCurrencyMinAmount\":" + randomSoftCurrencyMinAmount + "," +
            "\"randomSoftCurrencyMaxAmount\":" + randomSoftCurrencyMaxAmount + "," +
            "\"rewardPlayerExp\":" + rewardPlayerExp + "," +
            "\"rewardCharacterExp\":" + rewardCharacterExp + "," +
            "\"rewardItems\":" + jsonRewardItems + "," +
            "\"firstClearRewardSoftCurrency\":" + firstClearRewardSoftCurrency + "," +
            "\"firstClearRewardHardCurrency\":" + firstClearRewardHardCurrency + "," +
            "\"firstClearRewardPlayerExp\":" + firstClearRewardPlayerExp + "," +
            "\"firstClearRewardItems\":" + jsonFirstClearRewardItems + "," +
            "\"unlockStages\":" + jsonUnlockStages + "}";
    }
}
