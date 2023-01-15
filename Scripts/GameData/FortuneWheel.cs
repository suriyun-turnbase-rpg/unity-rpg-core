using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FortuneWheelReward
{
    public ItemAmount[] rewardItems;
    public int rewardSoftCurrency;
    public int rewardHardCurrency;
    public int randomWeight;
    [HideInInspector]
    public int rewardIndex;

    public string ToJson()
    {
        // Reward Items
        var jsonRewardItems = "";
        if (rewardItems != null)
        {
            foreach (var entry in rewardItems)
            {
                if (!string.IsNullOrEmpty(jsonRewardItems))
                    jsonRewardItems += ",";
                jsonRewardItems += entry.ToJson();
            }
        }
        jsonRewardItems = "[" + jsonRewardItems + "]";
        return "{\"rewardSoftCurrency\":" + rewardSoftCurrency + "," +
            "\"rewardHardCurrency\":" + rewardHardCurrency + "," +
            "\"rewardItems\":" + jsonRewardItems + "," +
            "\"randomWeight\":" + randomWeight + "}";
    }
}

public enum FortuneWheelRequirementType : byte
{
    RequireSoftCurrency = 0,
    RequireHardCurrency = 1,
}

public class FortuneWheel : BaseGameData
{
    public FortuneWheelRequirementType requirementType;
    public int price = 0;
    public FortuneWheelReward[] rewards;

    public FortuneWheelReward RandomReward()
    {
        var weight = new Dictionary<FortuneWheelReward, int>();
        for (int i = 0; i < rewards.Length; ++i)
        {
            rewards[i].rewardIndex = i;
            weight.Add(rewards[i], rewards[i].randomWeight);
        }
        return WeightedRandomizer.From(weight).TakeOne();
    }

    public virtual string ToJson()
    {
        // Reward Items
        var jsonRewards = "";
        if (rewards != null)
        {
            foreach (var entry in rewards)
            {
                if (!string.IsNullOrEmpty(jsonRewards))
                    jsonRewards += ",";
                jsonRewards += entry.ToJson();
            }
        }
        jsonRewards = "[" + jsonRewards + "]";
        return "{\"id\":\"" + Id + "\"," +
            "\"requirementType\":" + (byte)requirementType + "," +
            "\"price\":" + price + "," +
            "\"rewards\":" + jsonRewards + "}";
    }
}
