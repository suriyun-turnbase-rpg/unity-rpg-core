using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine.Events;

public partial class SQLiteGameService
{
    protected override void DoGetUnlockIconList(string playerId, string loginToken, UnityAction<UnlockIconListResult> onFinish)
    {
        var result = new UnlockIconListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerUnlockIcons(playerId);
        onFinish(result);
    }

    protected List<PlayerUnlockIcon> GetPlayerUnlockIcons(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerUnlockIcon WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerUnlockIcon>();
        while (reader.Read())
        {
            var entry = new PlayerUnlockIcon();
            entry.Id = reader.GetString("id");
            entry.PlayerId = reader.GetString("playerId");
            entry.DataId = reader.GetString("dataId");
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetUnlockFrameList(string playerId, string loginToken, UnityAction<UnlockFrameListResult> onFinish)
    {
        var result = new UnlockFrameListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerUnlockFrames(playerId);
        onFinish(result);
    }

    protected List<PlayerUnlockFrame> GetPlayerUnlockFrames(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerUnlockFrame WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerUnlockFrame>();
        while (reader.Read())
        {
            var entry = new PlayerUnlockFrame();
            entry.Id = reader.GetString("id");
            entry.PlayerId = reader.GetString("playerId");
            entry.DataId = reader.GetString("dataId");
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetUnlockTitleList(string playerId, string loginToken, UnityAction<UnlockTitleListResult> onFinish)
    {
        var result = new UnlockTitleListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerUnlockTitles(playerId);
        onFinish(result);
    }

    protected List<PlayerUnlockTitle> GetPlayerUnlockTitles(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerUnlockTitle WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerUnlockTitle>();
        while (reader.Read())
        {
            var entry = new PlayerUnlockTitle();
            entry.Id = reader.GetString("id");
            entry.PlayerId = reader.GetString("playerId");
            entry.DataId = reader.GetString("dataId");
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetClanUnlockIconList(string playerId, string loginToken, UnityAction<ClanUnlockIconListResult> onFinish)
    {
        var result = new ClanUnlockIconListResult();
        var reader = ExecuteReader(@"SELECT clanId FROM player WHERE id=@playerId AND loginToken=@loginToken LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (!reader.Read())
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetClanUnlockIcons(reader.GetString("clanId"));
        onFinish(result);
    }

    protected List<ClanUnlockIcon> GetClanUnlockIcons(string clanId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerUnlockIcon WHERE clanId=@clanId", new SqliteParameter("@clanId", clanId));
        var list = new List<ClanUnlockIcon>();
        while (reader.Read())
        {
            var entry = new ClanUnlockIcon();
            entry.Id = reader.GetString("id");
            entry.ClanId = reader.GetString("clanId");
            entry.DataId = reader.GetString("dataId");
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetClanUnlockFrameList(string playerId, string loginToken, UnityAction<ClanUnlockFrameListResult> onFinish)
    {
        var result = new ClanUnlockFrameListResult();
        var reader = ExecuteReader(@"SELECT clanId FROM player WHERE id=@playerId AND loginToken=@loginToken LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (!reader.Read())
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetClanUnlockFrames(reader.GetString("clanId"));
        onFinish(result);
    }

    protected List<ClanUnlockFrame> GetClanUnlockFrames(string clanId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerUnlockFrame WHERE clanId=@clanId", new SqliteParameter("@clanId", clanId));
        var list = new List<ClanUnlockFrame>();
        while (reader.Read())
        {
            var entry = new ClanUnlockFrame();
            entry.Id = reader.GetString("id");
            entry.ClanId = reader.GetString("clanId");
            entry.DataId = reader.GetString("dataId");
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetClanUnlockTitleList(string playerId, string loginToken, UnityAction<ClanUnlockTitleListResult> onFinish)
    {
        var result = new ClanUnlockTitleListResult();
        var reader = ExecuteReader(@"SELECT clanId FROM player WHERE id=@playerId AND loginToken=@loginToken LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (!reader.Read())
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetClanUnlockTitles(reader.GetString("clanId"));
        onFinish(result);
    }

    protected List<ClanUnlockTitle> GetClanUnlockTitles(string clanId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerUnlockTitle WHERE clanId=@clanId", new SqliteParameter("@clanId", clanId));
        var list = new List<ClanUnlockTitle>();
        while (reader.Read())
        {
            var entry = new ClanUnlockTitle();
            entry.Id = reader.GetString("id");
            entry.ClanId = reader.GetString("clanId");
            entry.DataId = reader.GetString("dataId");
            list.Add(entry);
        }
        return list;
    }
}
