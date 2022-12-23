using System.Collections.Generic;

[System.Serializable]
public class DbClanUnlockFrame : IClanUnlockFrame
{
    public string id;
    [LiteDB.BsonId]
    public string Id { get { return id; } set { id = value; } }
    public string clanId;
    public string ClanId { get { return clanId; } set { clanId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public static List<ClanUnlockFrame> CloneList(IEnumerable<DbClanUnlockFrame> list)
    {
        var result = new List<ClanUnlockFrame>();
        foreach (var entry in list)
        {
            result.Add(ClanUnlockFrame.CloneTo(entry, new ClanUnlockFrame()));
        }
        return result;
    }
}
