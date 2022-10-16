using System.Collections.Generic;

[System.Serializable]
public class DbPlayerUnlockTitle : IPlayerUnlockTitle
{
    public string id;
    [LiteDB.BsonId]
    public string Id { get { return id; } set { id = value; } }
    public string playerId;
    public string PlayerId { get { return playerId; } set { playerId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public static List<PlayerUnlockTitle> CloneList(IEnumerable<DbPlayerUnlockTitle> list)
    {
        var result = new List<PlayerUnlockTitle>();
        foreach (var entry in list)
        {
            result.Add(PlayerUnlockTitle.CloneTo(entry, new PlayerUnlockTitle()));
        }
        return result;
    }
}
