using System.Collections.Generic;

[System.Serializable]
public class ClanUnlockFrame : BasePlayerData, IClanUnlockFrame
{
    public static readonly Dictionary<string, ClanUnlockFrame> DataMap = new Dictionary<string, ClanUnlockFrame>();
    public string Id { get { return GetId(ClanId, DataId); } set { } }
    public string clanId;
    public string ClanId { get { return clanId; } set { clanId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public ClanUnlockFrame Clone()
    {
        return CloneTo(this, new ClanUnlockFrame());
    }

    public static T CloneTo<T>(IClanUnlockFrame from, T to) where T : IClanUnlockFrame
    {
        to.Id = from.Id;
        to.ClanId = from.ClanId;
        to.DataId = from.DataId;
        return to;
    }

    public static string GetId(string clanId, string dataId)
    {
        return clanId + "_" + dataId;
    }

    public static void SetData(ClanUnlockFrame data)
    {
        if (data == null || string.IsNullOrEmpty(data.Id))
            return;
        DataMap[data.Id] = data;
    }

    public static bool TryGetData(string clanId, string dataId, out ClanUnlockFrame data)
    {
        return DataMap.TryGetValue(GetId(clanId, dataId), out data);
    }

    public static bool TryGetData(string dataId, out ClanUnlockFrame data)
    {
        return TryGetData(Player.CurrentPlayer.ClanId, dataId, out data);
    }

    public static bool RemoveData(string id)
    {
        return DataMap.Remove(id);
    }

    public static void ClearData()
    {
        DataMap.Clear();
    }

    public static void SetDataRange(IEnumerable<ClanUnlockFrame> list)
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

    public static void RemoveDataRange(string clanId)
    {
        var values = DataMap.Values;
        foreach (var value in values)
        {
            if (value.ClanId == clanId)
                RemoveData(value.Id);
        }
    }

    public static void RemoveDataRange()
    {
        RemoveDataRange(Player.CurrentPlayer.ClanId);
    }

    public static bool IsUnlock(string clanId, string dataId)
    {
        var Id = GetId(clanId, dataId);
        if (DataMap.ContainsKey(Id))
            return true;
        return false;
    }

    public static bool IsUnlock(string clanId, GenericUnlockable data)
    {
        return IsUnlock(clanId, data);
    }

    public static bool IsUnlock(string dataId)
    {
        return IsUnlock(Player.CurrentPlayer.ClanId, dataId);
    }

    public static bool IsUnlock(GenericUnlockable data)
    {
        return IsUnlock(Player.CurrentPlayer.ClanId, data);
    }
}
