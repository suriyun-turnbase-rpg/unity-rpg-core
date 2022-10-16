using System.Collections.Generic;

[System.Serializable]
public class DbPlayerUnlockFrame : IPlayerUnlockFrame
{
    public string id;
    [LiteDB.BsonId]
    public string Id { get { return id; } set { id = value; } }
    public string playerId;
    public string PlayerId { get { return playerId; } set { playerId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public static List<PlayerUnlockFrame> CloneList(IEnumerable<DbPlayerUnlockFrame> list)
    {
        var result = new List<PlayerUnlockFrame>();
        foreach (var entry in list)
        {
            result.Add(PlayerUnlockFrame.CloneTo(entry, new PlayerUnlockFrame()));
        }
        return result;
    }
}
