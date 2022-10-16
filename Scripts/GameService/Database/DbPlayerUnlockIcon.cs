using System.Collections.Generic;

[System.Serializable]
public class DbPlayerUnlockIcon : IPlayerUnlockIcon
{
    public string id;
    [LiteDB.BsonId]
    public string Id { get { return id; } set { id = value; } }
    public string playerId;
    public string PlayerId { get { return playerId; } set { playerId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public static List<PlayerUnlockIcon> CloneList(IEnumerable<DbPlayerUnlockIcon> list)
    {
        var result = new List<PlayerUnlockIcon>();
        foreach (var entry in list)
        {
            result.Add(PlayerUnlockIcon.CloneTo(entry, new PlayerUnlockIcon()));
        }
        return result;
    }
}
