using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Mono.Data.Sqlite;
using System.Data;

public class DbRowsReader
{
    private readonly List<List<object>> data = new List<List<object>>();
    private readonly List<Dictionary<string, object>> dataDict = new List<Dictionary<string, object>>();
    private int currentRow = -1;
    public int FieldCount { get; private set; }
    public int VisibleFieldCount { get; private set; }
    public int RowCount { get { return data.Count; } }
    public bool HasRows { get { return RowCount > 0; } }

    public void Init(SqliteDataReader dataReader)
    {
        data.Clear();
        dataDict.Clear();
        FieldCount = dataReader.FieldCount;
        VisibleFieldCount = dataReader.VisibleFieldCount;
        while (dataReader.Read())
        {
            List<object> row = new List<object>();
            Dictionary<string, object> rowDict = new Dictionary<string, object>();
            for (int i = 0; i < FieldCount; ++i)
            {
                string fieldName = dataReader.GetName(i);
                object value = dataReader.GetValue(i);
                row.Add(value);
                rowDict.Add(fieldName, value);
            }
            data.Add(row);
            dataDict.Add(rowDict);
        }
    }

    public bool Read()
    {
        if (currentRow + 1 >= RowCount)
            return false;
        ++currentRow;
        return true;
    }

    public System.DateTime GetDateTime(int index)
    {
        return (System.DateTime)data[currentRow][index];
    }

    public System.DateTime GetDateTime(string columnName)
    {
        return (System.DateTime)dataDict[currentRow][columnName];
    }

    public object GetObject(int index)
    {
        return data[currentRow][index];
    }

    public object GetObject(string columnName)
    {
        return dataDict[currentRow][columnName];
    }

    public byte GetByte(int index)
    {
        return (byte)(long)data[currentRow][index];
    }

    public byte GetByte(string columnName)
    {
        return (byte)(long)dataDict[currentRow][columnName];
    }

    public sbyte GetSByte(int index)
    {
        return (sbyte)(long)data[currentRow][index];
    }

    public sbyte GetSByte(string columnName)
    {
        return (sbyte)(long)dataDict[currentRow][columnName];
    }

    public char GetChar(int index)
    {
        return (char)data[currentRow][index];
    }

    public char GetChar(string columnName)
    {
        return (char)dataDict[currentRow][columnName];
    }

    public string GetString(int index)
    {
        return (string)data[currentRow][index];
    }

    public string GetString(string columnName)
    {
        return (string)dataDict[currentRow][columnName];
    }

    public bool GetBoolean(int index)
    {
        return ((long)data[currentRow][index]) != 0;
    }

    public bool GetBoolean(string columnName)
    {
        return ((long)dataDict[currentRow][columnName]) != 0;
    }

    public short GetInt16(int index)
    {
        try { return (short)(long)data[currentRow][index]; } catch { return 0; }
    }

    public short GetInt16(string columnName)
    {
        try { return (short)(long)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public int GetInt32(int index)
    {
        try { return (int)(long)data[currentRow][index]; } catch { return 0; }
    }

    public int GetInt32(string columnName)
    {
        try { return (int)(long)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public long GetInt64(int index)
    {
        try { return (long)data[currentRow][index]; } catch { return 0; }
    }

    public long GetInt64(string columnName)
    {
        try { return (long)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public ushort GetUInt16(int index)
    {
        try { return (ushort)(long)data[currentRow][index]; } catch { return 0; }
    }

    public ushort GetUInt16(string columnName)
    {
        try { return (ushort)(long)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public uint GetUInt32(int index)
    {
        try { return (uint)(long)data[currentRow][index]; } catch { return 0; }
    }

    public uint GetUInt32(string columnName)
    {
        try { return (uint)(long)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public ulong GetUInt64(int index)
    {
        try { return (ulong)(long)data[currentRow][index]; } catch { return 0; }
    }

    public ulong GetUInt64(string columnName)
    {
        try { return (ulong)(long)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public decimal GetDecimal(int index)
    {
        try { return (decimal)(float)data[currentRow][index]; } catch { return 0; }
    }

    public decimal GetDecimal(string columnName)
    {
        try { return (decimal)(float)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public float GetFloat(int index)
    {
        try { return (float)data[currentRow][index]; } catch { return 0; }
    }

    public float GetFloat(string columnName)
    {
        try { return (float)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public double GetDouble(int index)
    {
        try { return (float)data[currentRow][index]; } catch { return 0; }
    }

    public double GetDouble(string columnName)
    {
        try { return (float)dataDict[currentRow][columnName]; } catch { return 0; }
    }

    public void ResetReader()
    {
        currentRow = -1;
    }
}

public partial class SQLiteGameService : BaseGameService
{
    public string dbPath = "./tbRpgDb.sqlite3";

    private SqliteConnection connection;

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            if (dbPath.StartsWith("./"))
                dbPath = dbPath.Substring(1);
            if (!dbPath.StartsWith("/"))
                dbPath = "/" + dbPath;
            dbPath = Application.persistentDataPath + dbPath;
        }

        if (!File.Exists(dbPath))
            SqliteConnection.CreateFile(dbPath);

        // open connection
        connection = new SqliteConnection("URI=file:" + dbPath);

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS player (
            id TEXT NOT NULL PRIMARY KEY,
            profileName TEXT NOT NULL,
            loginToken TEXT NOT NULL,
            exp INTEGER NOT NULL,
            selectedFormation TEXT NOT NULL DEFAULT '',
            selectedArenaFormation TEXT NOT NULL DEFAULT '',
            arenaScore INTEGER NOT NULL DEFAULT 0,
            highestArenaRank INTEGER NOT NULL DEFAULT 0,
            highestArenaRankCurrentSeason INTEGER NOT NULL DEFAULT 0)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerItem (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            dataId TEXT NOT NULL,
            amount INTEGER NOT NULL,
            exp INTEGER NOT NULL,
            equipItemId TEXT NOT NULL,
            equipPosition TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerAchievement (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            dataId TEXT NOT NULL,
            progress INTEGER NOT NULL,
            earned INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerAuth (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            type TEXT NOT NULL,
            username TEXT NOT NULL,
            password TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerCurrency (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            dataId TEXT NOT NULL,
            amount INTEGER NOT NULL,
            purchasedAmount INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerStamina (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            dataId TEXT NOT NULL,
            amount INTEGER NOT NULL,
            recoveredTime INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerFormation (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            dataId TEXT NOT NULL,
            position INTEGER NOT NULL,
            itemId TEXT NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerUnlockItem (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            dataId TEXT NOT NULL,
            amount INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerClearStage (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            dataId TEXT NOT NULL,
            bestRating INTEGER NOT NULL)");

        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS playerBattle (
            id TEXT NOT NULL PRIMARY KEY,
            playerId TEXT NOT NULL,
            dataId TEXT NOT NULL,
            session TEXT NOT NULL,
            battleResult INTEGER NOT NULL,
            rating INTEGER NOT NULL,
            battleType INTEGER NOT NULL DEFAULT 0)");

        if (!IsColumnExist("player", "arenaScore"))
            ExecuteNonQuery("ALTER TABLE player ADD arenaScore INTEGER NOT NULL DEFAULT 0;");
            
        if (!IsColumnExist("player", "highestArenaRank"))
            ExecuteNonQuery("ALTER TABLE player ADD highestArenaRank INTEGER NOT NULL DEFAULT 0;");
            
        if (!IsColumnExist("player", "highestArenaRankCurrentSeason"))
            ExecuteNonQuery("ALTER TABLE player ADD highestArenaRankCurrentSeason INTEGER NOT NULL DEFAULT 0;");

        if (!IsColumnExist("player", "selectedArenaFormation"))
            ExecuteNonQuery("ALTER TABLE player ADD selectedArenaFormation TEXT NOT NULL DEFAULT '';");

        if (!IsColumnExist("playerBattle", "battleType"))
            ExecuteNonQuery("ALTER TABLE playerBattle ADD battleType INTEGER NOT NULL DEFAULT 0;");
    }

    private bool IsColumnExist(string tableName, string findingColumn)
    {
        using (SqliteCommand cmd = new SqliteCommand("PRAGMA table_info(" + tableName + ");", connection))
        {
            DataTable table = new DataTable();

            SqliteDataAdapter adp = null;
            try
            {
                adp = new SqliteDataAdapter(cmd);
                adp.Fill(table);
                for (int i = 0; i < table.Rows.Count; ++i)
                {
                    if (table.Rows[i]["name"].ToString().Equals(findingColumn))
                        return true;
                }
            }
            catch { }
        }
        return false;
    }

    public void ExecuteNonQuery(string sql, params SqliteParameter[] args)
    {
        connection.Open();
        using (var cmd = new SqliteCommand(sql, connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            cmd.ExecuteNonQuery();
        }
        connection.Close();
    }

    public object ExecuteScalar(string sql, params SqliteParameter[] args)
    {
        object result;
        connection.Open();
        using (var cmd = new SqliteCommand(sql, connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            result = cmd.ExecuteScalar();
        }
        connection.Close();
        return result;
    }

    public DbRowsReader ExecuteReader(string sql, params SqliteParameter[] args)
    {
        DbRowsReader result = new DbRowsReader();
        connection.Open();
        using (var cmd = new SqliteCommand(sql, connection))
        {
            foreach (var arg in args)
            {
                cmd.Parameters.Add(arg);
            }
            result.Init(cmd.ExecuteReader());
        }
        connection.Close();
        return result;
    }

    protected override void DoGetAchievementList(string playerId, string loginToken, UnityAction<AchievementListResult> onFinish)
    {
        var result = new AchievementListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerAchievements(playerId);
        onFinish(result);
    }

    protected List<PlayerAchievement> GetPlayerAchievements(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerAchievement WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerAchievement>();
        while (reader.Read())
        {
            var entry = new PlayerAchievement();
            entry.Id = reader.GetString(0);
            entry.PlayerId = reader.GetString(1);
            entry.DataId = reader.GetString(2);
            entry.Progress = reader.GetInt32(3);
            entry.Earned = reader.GetInt32(4) > 0;
            list.Add(entry);
        }
        return list;
    }

    protected PlayerAchievement GetPlayerAchievement(string playerId, string dataId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerAchievement WHERE playerId=@playerId AND dataId=@dataId LIMIT 1",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@dataId", dataId));
        PlayerAchievement result = null;
        if (reader.Read())
        {
            result = new PlayerAchievement();
            result.Id = reader.GetString(0);
            result.PlayerId = reader.GetString(1);
            result.DataId = reader.GetString(2);
            result.Progress = reader.GetInt32(3);
            result.Earned = reader.GetInt32(4) > 0;
        }
        return result;
    }

    protected override void DoGetAuthList(string playerId, string loginToken, UnityAction<AuthListResult> onFinish)
    {
        var result = new AuthListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerAuths(playerId);
        onFinish(result);
    }

    protected List<PlayerAuth> GetPlayerAuths(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerAuth WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerAuth>();
        while (reader.Read())
        {
            var entry = new PlayerAuth();
            entry.Id = reader.GetString(0);
            entry.PlayerId = reader.GetString(1);
            entry.Type = reader.GetString(2);
            entry.Username = reader.GetString(3);
            entry.Password = reader.GetString(4);
            list.Add(entry);
        }
        return list;
    }


    protected override void DoGetItemList(string playerId, string loginToken, UnityAction<ItemListResult> onFinish)
    {
        var result = new ItemListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerItems(playerId);
        onFinish(result);
    }

    protected List<PlayerItem> GetPlayerItems(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerItem WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerItem>();
        while (reader.Read())
        {
            var entry = new PlayerItem();
            entry.Id = reader.GetString(0);
            entry.PlayerId = reader.GetString(1);
            entry.DataId = reader.GetString(2);
            entry.Amount = reader.GetInt32(3);
            entry.Exp = reader.GetInt32(4);
            entry.EquipItemId = reader.GetString(5);
            entry.EquipPosition = reader.GetString(6);
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetCurrencyList(string playerId, string loginToken, UnityAction<CurrencyListResult> onFinish)
    {
        var result = new CurrencyListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerCurrencies(playerId);
        onFinish(result);
    }

    protected List<PlayerCurrency> GetPlayerCurrencies(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerCurrency WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerCurrency>();
        while (reader.Read())
        {
            var entry = new PlayerCurrency();
            entry.Id = reader.GetString(0);
            entry.PlayerId = reader.GetString(1);
            entry.DataId = reader.GetString(2);
            entry.Amount = reader.GetInt32(3);
            entry.PurchasedAmount = reader.GetInt32(4);
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetStaminaList(string playerId, string loginToken, UnityAction<StaminaListResult> onFinish)
    {
        var result = new StaminaListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerStaminas(playerId);
        onFinish(result);
    }

    protected List<PlayerStamina> GetPlayerStaminas(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerStamina WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerStamina>();
        while (reader.Read())
        {
            var entry = new PlayerStamina();
            entry.Id = reader.GetString(0);
            entry.PlayerId = reader.GetString(1);
            entry.DataId = reader.GetString(2);
            entry.Amount = reader.GetInt32(3);
            entry.RecoveredTime = reader.GetInt64(4);
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetFormationList(string playerId, string loginToken, UnityAction<FormationListResult> onFinish)
    {
        var result = new FormationListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerFormations(playerId);
        onFinish(result);
    }

    protected List<PlayerFormation> GetPlayerFormations(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerFormation WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerFormation>();
        while (reader.Read())
        {
            var entry = new PlayerFormation();
            entry.Id = reader.GetString(0);
            entry.PlayerId = reader.GetString(1);
            entry.DataId = reader.GetString(2);
            entry.Position = reader.GetInt32(3);
            entry.ItemId = reader.GetString(4);
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetUnlockItemList(string playerId, string loginToken, UnityAction<UnlockItemListResult> onFinish)
    {
        var result = new UnlockItemListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerUnlockItems(playerId);
        onFinish(result);
    }

    protected List<PlayerUnlockItem> GetPlayerUnlockItems(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerUnlockItem WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerUnlockItem>();
        while (reader.Read())
        {
            var entry = new PlayerUnlockItem();
            entry.Id = reader.GetString(0);
            entry.PlayerId = reader.GetString(1);
            entry.DataId = reader.GetString(2);
            entry.Amount = reader.GetInt32(3);
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetClearStageList(string playerId, string loginToken, UnityAction<ClearStageListResult> onFinish)
    {
        var result = new ClearStageListResult();
        var player = ExecuteScalar(@"SELECT COUNT(*) FROM player WHERE id=@playerId AND loginToken=@loginToken",
            new SqliteParameter("@playerId", playerId),
            new SqliteParameter("@loginToken", loginToken));
        if (player == null || (long)player <= 0)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list = GetPlayerClearStages(playerId);
        onFinish(result);
    }

    protected List<PlayerClearStage> GetPlayerClearStages(string playerId)
    {
        var reader = ExecuteReader(@"SELECT * FROM playerClearStage WHERE playerId=@playerId", new SqliteParameter("@playerId", playerId));
        var list = new List<PlayerClearStage>();
        while (reader.Read())
        {
            var entry = new PlayerClearStage();
            entry.Id = reader.GetString(0);
            entry.PlayerId = reader.GetString(1);
            entry.DataId = reader.GetString(2);
            entry.BestRating = reader.GetInt32(3);
            list.Add(entry);
        }
        return list;
    }

    protected override void DoGetServiceTime(UnityAction<ServiceTimeResult> onFinish)
    {
        var result = new ServiceTimeResult();
        result.serviceTime = Timestamp;
        onFinish(result);
    }

    protected PlayerItem QueryCreatePlayerItem(PlayerItem playerItem)
    {
        playerItem.Id = System.Guid.NewGuid().ToString();
        ExecuteNonQuery(@"INSERT INTO playerItem (id, playerId, dataId, amount, exp, equipItemId, equipPosition) VALUES (@id, @playerId, @dataId, @amount, @exp, @equipItemId, @equipPosition)",
            new SqliteParameter("@id", playerItem.Id),
            new SqliteParameter("@playerId", playerItem.PlayerId),
            new SqliteParameter("@dataId", playerItem.DataId),
            new SqliteParameter("@amount", playerItem.Amount),
            new SqliteParameter("@exp", playerItem.Exp),
            new SqliteParameter("@equipItemId", playerItem.EquipItemId),
            new SqliteParameter("@equipPosition", playerItem.EquipPosition));
        return playerItem;
    }

    protected PlayerItem QueryUpdatePlayerItem(PlayerItem playerItem)
    {
        ExecuteNonQuery(@"UPDATE playerItem SET playerId=@playerId, dataId=@dataId, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
            new SqliteParameter("@playerId", playerItem.PlayerId),
            new SqliteParameter("@dataId", playerItem.DataId),
            new SqliteParameter("@amount", playerItem.Amount),
            new SqliteParameter("@exp", playerItem.Exp),
            new SqliteParameter("@equipItemId", playerItem.EquipItemId),
            new SqliteParameter("@equipPosition", playerItem.EquipPosition),
            new SqliteParameter("@id", playerItem.Id));
        return playerItem;
    }

    protected PlayerAchievement QueryCreatePlayerAchievement(PlayerAchievement playerAchievement)
    {
        playerAchievement.Id = System.Guid.NewGuid().ToString();
        ExecuteNonQuery(@"INSERT INTO playerAchievement (id, playerId, dataId, progress, earned) VALUES (@id, @playerId, @dataId, @progress, @earned)",
            new SqliteParameter("@id", playerAchievement.Id),
            new SqliteParameter("@playerId", playerAchievement.PlayerId),
            new SqliteParameter("@dataId", playerAchievement.DataId),
            new SqliteParameter("@progress", playerAchievement.Progress),
            new SqliteParameter("@earned", playerAchievement.Earned ? 1 : 0));
        return playerAchievement;
    }

    protected PlayerAchievement QueryUpdatePlayerAchievement(PlayerAchievement playerAchievement)
    {
        ExecuteNonQuery(@"UPDATE playerAchievement SET playerId=@playerId, dataId=@dataId, progress=@progress, earned=@earned WHERE id=@id",
            new SqliteParameter("@playerId", playerAchievement.PlayerId),
            new SqliteParameter("@dataId", playerAchievement.DataId),
            new SqliteParameter("@progress", playerAchievement.Progress),
            new SqliteParameter("@earned", playerAchievement.Earned ? 1 : 0),
            new SqliteParameter("@id", playerAchievement.Id));
        return playerAchievement;
    }
}
