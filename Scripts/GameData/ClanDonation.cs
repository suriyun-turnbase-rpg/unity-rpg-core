using UnityEngine;

[System.Serializable]
public struct ClanDonation
{
    public string id;
    public Sprite icon;
    public string requireCurrencyId;
    public int requireCurrencyAmount;
    public int rewardClanExp;

    public string ToJson()
    {
        return "{\"id\":\"" + id + "\"," +
            "\"requireCurrencyId\":" + requireCurrencyId + "," +
            "\"requireCurrencyAmount\":" + requireCurrencyAmount + "," +
            "\"rewardClanExp\":" + rewardClanExp + "}";
    }
}
