public class DailyReward : BaseGameData
{
    public DailyRewardMode mode = DailyRewardMode.Weekly;
    public bool consecutive = false;
    public RewardData[] rewards = new RewardData[0];

    public virtual string ToJson()
    {
        // Rewards
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
            "\"mode\":" + (byte)mode + "," +
            "\"consecutive\":" + (consecutive ? "true" : "false") + "," +
            "\"rewards\":" + jsonRewards + "}";
    }
}
