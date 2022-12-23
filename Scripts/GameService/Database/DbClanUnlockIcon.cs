using System.Collections.Generic;

[System.Serializable]
public class DbClanUnlockIcon : IClanUnlockIcon
{
    public string id;
    [LiteDB.BsonId]
    public string Id { get { return id; } set { id = value; } }
    public string clanId;
    public string ClanId { get { return clanId; } set { clanId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public static List<ClanUnlockIcon> CloneList(IEnumerable<DbClanUnlockIcon> list)
    {
        var result = new List<ClanUnlockIcon>();
        foreach (var entry in list)
        {
            result.Add(ClanUnlockIcon.CloneTo(entry, new ClanUnlockIcon()));
        }
        return result;
    }
}
