using System.Collections.Generic;

[System.Serializable]
public class DbClanUnlockTitle : IClanUnlockTitle
{
    public string id;
    [LiteDB.BsonId]
    public string Id { get { return id; } set { id = value; } }
    public string clanId;
    public string ClanId { get { return clanId; } set { clanId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public static List<ClanUnlockTitle> CloneList(IEnumerable<DbClanUnlockTitle> list)
    {
        var result = new List<ClanUnlockTitle>();
        foreach (var entry in list)
        {
            result.Add(ClanUnlockTitle.CloneTo(entry, new ClanUnlockTitle()));
        }
        return result;
    }
}
