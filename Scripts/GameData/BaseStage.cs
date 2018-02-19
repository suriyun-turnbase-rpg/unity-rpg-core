using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStage : BaseGameData
{
    public string stageNumber;
    public Sprite icon;
    [Header("Stamina")]
    public int requireStamina;
    [Header("Rewards")]
    public int randomSoftCurrencyMinAmount;
    public int randomSoftCurrencyMaxAmount;
    public int rewardPlayerExp;
    public int rewardCharacterExp;
    public ItemDrop[] rewardItems;
    [Header("Unlock")]
    public Stage[] unlockStages;

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
            "\"requireStamina\":" + requireStamina + "," +
            "\"randomSoftCurrencyMinAmount\":" + randomSoftCurrencyMinAmount + "," +
            "\"randomSoftCurrencyMaxAmount\":" + randomSoftCurrencyMaxAmount + "," +
            "\"rewardPlayerExp\":" + rewardPlayerExp + "," +
            "\"rewardCharacterExp\":" + rewardCharacterExp + "," +
            "\"rewardItems\":" + jsonRewardItems + "," +
            "\"unlockStages\":" + jsonUnlockStages + "}";
    }
}
