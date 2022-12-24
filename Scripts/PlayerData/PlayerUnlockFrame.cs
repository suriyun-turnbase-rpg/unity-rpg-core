using System.Collections.Generic;

[System.Serializable]
public class PlayerUnlockFrame : BasePlayerData, IPlayerUnlockFrame
{
    public static readonly Dictionary<string, PlayerUnlockFrame> DataMap = new Dictionary<string, PlayerUnlockFrame>();
    public string Id { get { return GetId(PlayerId, DataId); } set { } }
    public string playerId;
    public string PlayerId { get { return playerId; } set { playerId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public PlayerUnlockFrame Clone()
    {
        return CloneTo(this, new PlayerUnlockFrame());
    }

    public static T CloneTo<T>(IPlayerUnlockFrame from, T to) where T : IPlayerUnlockFrame
    {
        to.Id = from.Id;
        to.PlayerId = from.PlayerId;
        to.DataId = from.DataId;
        return to;
    }

    public static string GetId(string playerId, string dataId)
    {
        return playerId + "_" + dataId;
    }

    public static void SetData(PlayerUnlockFrame data)
    {
        if (data == null || string.IsNullOrEmpty(data.Id))
            return;
        DataMap[data.Id] = data;
    }

    public static bool TryGetData(string playerId, string dataId, out PlayerUnlockFrame data)
    {
        return DataMap.TryGetValue(GetId(playerId, dataId), out data);
    }

    public static bool TryGetData(string dataId, out PlayerUnlockFrame data)
    {
        return TryGetData(Player.CurrentPlayerId, dataId, out data);
    }

    public static bool RemoveData(string id)
    {
        return DataMap.Remove(id);
    }

    public static void ClearData()
    {
        DataMap.Clear();
    }

    public static void SetDataRange(IEnumerable<PlayerUnlockFrame> list)
    {
        foreach (var data in list)
        {
            SetData(data);
        }
    }

    public static void RemoveDataRange(IEnumerable<string> ids)
    {
        foreach (var id in ids)
        {
            RemoveData(id);
        }
    }

    public static void RemoveDataRange(string playerId)
    {
        var values = DataMap.Values;
        foreach (var value in values)
        {
            if (value.PlayerId == playerId)
                RemoveData(value.Id);
        }
    }

    public static void RemoveDataRange()
    {
        RemoveDataRange(Player.CurrentPlayerId);
    }

    public static bool IsUnlock(string playerId, string dataId)
    {
        var Id = GetId(playerId, dataId);
        if (DataMap.ContainsKey(Id))
            return true;
        return false;
    }

    public static bool IsUnlock(string playerId, GenericUnlockable data)
    {
        return IsUnlock(playerId, data.Id);
    }

    public static bool IsUnlock(string dataId)
    {
        return IsUnlock(Player.CurrentPlayerId, dataId);
    }

    public static bool IsUnlock(GenericUnlockable data)
    {
        return IsUnlock(Player.CurrentPlayerId, data);
    }
}
