[System.Serializable]
public struct RewardData
{
    public CurrencyAmount[] currencies;
    public ItemAmount[] items;

    public string ToJson()
    {
        // Reward Custom Currencies
        var jsonRewardCurrencies = "";
        if (currencies != null)
        {
            foreach (var entry in currencies)
            {
                if (!string.IsNullOrEmpty(jsonRewardCurrencies))
                    jsonRewardCurrencies += ",";
                jsonRewardCurrencies += entry.ToJson();
            }
        }
        jsonRewardCurrencies = "[" + jsonRewardCurrencies + "]";
        // Reward Items
        var jsonRewardItems = "";
        if (items != null)
        {
            foreach (var entry in items)
            {
                if (!string.IsNullOrEmpty(jsonRewardItems))
                    jsonRewardItems += ",";
                jsonRewardItems += entry.ToJson();
            }
        }
        jsonRewardItems = "[" + jsonRewardItems + "]";
        return "{\"rewardCurrencies\":" + jsonRewardCurrencies + ", \"rewardItems\":" + jsonRewardItems + "}";
    }
}
