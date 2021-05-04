[System.Serializable]
public struct RaidBossReward
{
    public int rankMin;
    public int rankMax;
    public CurrencyAmount[] rewardCustomCurrencies;
    public int rewardSoftCurrency;
    public int rewardHardCurrency;
    public ItemAmount[] rewardItems;

    public string ToJson()
    {
        // Reward custom currencies
        var jsonRewardCustomCurrencies = "";
        foreach (var entry in rewardCustomCurrencies)
        {
            if (!string.IsNullOrEmpty(jsonRewardCustomCurrencies))
                jsonRewardCustomCurrencies += ",";
            jsonRewardCustomCurrencies += entry.ToJson();
        }
        jsonRewardCustomCurrencies = "[" + jsonRewardCustomCurrencies + "]";
        // Reward items
        var jsonRewardItems = "";
        foreach (var entry in rewardItems)
        {
            if (!string.IsNullOrEmpty(jsonRewardItems))
                jsonRewardItems += ",";
            jsonRewardItems += entry.ToJson();
        }
        jsonRewardItems = "[" + jsonRewardItems + "]";
        return "{\"rankMin\":" + rankMin + "," +
            "\"rankMax\":" + rankMax + "," +
            "\"rewardCustomCurrencies\":" + jsonRewardCustomCurrencies + "," +
            "\"rewardSoftCurrency\":" + rewardSoftCurrency + "," +
            "\"rewardHardCurrency\":" + rewardHardCurrency + "," +
            "\"rewardItems\":" + jsonRewardItems + "}";
    }
}

public abstract class BaseRaidBossStage : BaseMission
{
    public RaidBossReward[] raidBossRewards;

    public abstract PlayerItem GetCharacter();

    public virtual string ToJson()
    {
        // Event Availibilities
        var jsonAvailabilities = "";
        foreach (var entry in availabilities)
        {
            if (!string.IsNullOrEmpty(jsonAvailabilities))
                jsonAvailabilities += ",";
            jsonAvailabilities += entry.ToJson();
        }
        jsonAvailabilities = "[" + jsonAvailabilities + "]";
        // Reward Custom Currencies
        var jsonRandomCustomCurrencies = "";
        foreach (var entry in randomCustomCurrencies)
        {
            if (!string.IsNullOrEmpty(jsonRandomCustomCurrencies))
                jsonRandomCustomCurrencies += ",";
            jsonRandomCustomCurrencies += entry.ToJson();
        }
        jsonRandomCustomCurrencies = "[" + jsonRandomCustomCurrencies + "]";
        // Reward Items
        var jsonRewardItems = "";
        foreach (var entry in rewardItems)
        {
            if (!string.IsNullOrEmpty(jsonRewardItems))
                jsonRewardItems += ",";
            jsonRewardItems += entry.ToJson();
        }
        jsonRewardItems = "[" + jsonRewardItems + "]";
        // First Clear Custom Currencies
        var jsonFirstClearRewardCustomCurrencies = "";
        foreach (var entry in firstClearRewardCustomCurrencies)
        {
            if (!string.IsNullOrEmpty(jsonFirstClearRewardCustomCurrencies))
                jsonFirstClearRewardCustomCurrencies += ",";
            jsonFirstClearRewardCustomCurrencies += entry.ToJson();
        }
        jsonFirstClearRewardCustomCurrencies = "[" + jsonFirstClearRewardCustomCurrencies + "]";
        // First Clear Reward Items
        var jsonFirstClearRewardItems = "";
        foreach (var entry in firstClearRewardItems)
        {
            if (!string.IsNullOrEmpty(jsonFirstClearRewardItems))
                jsonFirstClearRewardItems += ",";
            jsonFirstClearRewardItems += entry.ToJson();
        }
        jsonFirstClearRewardItems = "[" + jsonFirstClearRewardItems + "]";
        return "{\"id\":\"" + Id + "\"," +
            "\"stageType\":" + (int)stageType + "," +
            "\"recommendBattlePoint\":" + recommendBattlePoint + "," +
            "\"requireStamina\":" + requireStamina + "," +
            "\"requireCustomStamina\":\"" + requireCustomStamina + "\"," +
            "\"randomCustomCurrencies\":" + jsonRandomCustomCurrencies + "," +
            "\"randomSoftCurrencyMinAmount\":" + randomSoftCurrencyMinAmount + "," +
            "\"randomSoftCurrencyMaxAmount\":" + randomSoftCurrencyMaxAmount + "," +
            "\"rewardPlayerExp\":" + rewardPlayerExp + "," +
            "\"rewardClanExp\":" + rewardClanExp + "," +
            "\"rewardCharacterExp\":" + rewardCharacterExp + "," +
            "\"availabilities\":" + jsonAvailabilities + "," +
            "\"hasAvailableDate\":" + (hasAvailableDate ? 1 : 0) + "," +
            "\"startYear\":" + startYear + "," +
            "\"startMonth\":" + (int)startMonth + "," +
            "\"startDay\":" + startDay + "," +
            "\"durationDays\":" + durationDays + "," +
            "\"rewardItems\":" + jsonRewardItems + "," +
            "\"firstClearRewardCustomCurrencies\":" + jsonFirstClearRewardCustomCurrencies + "," +
            "\"firstClearRewardSoftCurrency\":" + firstClearRewardSoftCurrency + "," +
            "\"firstClearRewardHardCurrency\":" + firstClearRewardHardCurrency + "," +
            "\"firstClearRewardPlayerExp\":" + firstClearRewardPlayerExp + "," +
            "\"firstClearRewardItems\":" + jsonFirstClearRewardItems + "," +
            "\"maxHp\":" + GetCharacter().Attributes.hp + "}";
    }
}
