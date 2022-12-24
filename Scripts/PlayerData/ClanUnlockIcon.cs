using System.Collections.Generic;

[System.Serializable]
public class ClanUnlockIcon : BasePlayerData, IClanUnlockIcon
{
    public static readonly Dictionary<string, ClanUnlockIcon> DataMap = new Dictionary<string, ClanUnlockIcon>();
    public string Id { get { return GetId(ClanId, DataId); } set { } }
    public string clanId;
    public string ClanId { get { return clanId; } set { clanId = value; } }
    public string dataId;
    public string DataId { get { return dataId; } set { dataId = value; } }

    public ClanUnlockIcon Clone()
    {
        return CloneTo(this, new ClanUnlockIcon());
    }

    public static T CloneTo<T>(IClanUnlockIcon from, T to) where T : IClanUnlockIcon
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

    public static void SetData(ClanUnlockIcon data)
    {
        if (data == null || string.IsNullOrEmpty(data.Id))
            return;
        DataMap[data.Id] = data;
    }

    public static bool TryGetData(string clanId, string dataId, out ClanUnlockIcon data)
    {
        return DataMap.TryGetValue(GetId(clanId, dataId), out data);
    }

    public static bool TryGetData(string dataId, out ClanUnlockIcon data)
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

    public static void SetDataRange(IEnumerable<ClanUnlockIcon> list)
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
        return IsUnlock(clanId, data.Id);
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
