using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LiteDB;
using Newtonsoft.Json;

public partial class LiteDbGameService : BaseGameService
{
    public string dbPath = "./tbRpgDb.db";
    private LiteDatabase db;
    private LiteCollection<DbPlayer> colPlayer;
    private LiteCollection<DbPlayerItem> colPlayerItem;
    private LiteCollection<DbPlayerAchievement> colPlayerAchievement;
    private LiteCollection<DbPlayerAuth> colPlayerAuth;
    private LiteCollection<DbPlayerCurrency> colPlayerCurrency;
    private LiteCollection<DbPlayerStamina> colPlayerStamina;
    private LiteCollection<DbPlayerFormation> colPlayerFormation;
    private LiteCollection<DbPlayerUnlockItem> colPlayerUnlockItem;
    private LiteCollection<DbPlayerClearStage> colPlayerClearStage;
    private LiteCollection<DbPlayerBattle> colPlayerBattle;

    private void Awake()
    {
        BsonMapper.Global.RegisterType(
            (attributes) => JsonConvert.SerializeObject(attributes),
            (bson) => JsonConvert.DeserializeObject<CalculatedAttributes>(bson));

        if (Application.isMobilePlatform)
        {
            if (dbPath.StartsWith("./"))
                dbPath = dbPath.Substring(1);
            if (!dbPath.StartsWith("/"))
                dbPath = "/" + dbPath;
            dbPath = Application.persistentDataPath + dbPath;
        }
        db = new LiteDatabase("filename=" + dbPath + ";upgrade=true");
        colPlayer = db.GetCollection<DbPlayer>("player");
        colPlayerItem = db.GetCollection<DbPlayerItem>("playerItem");
        colPlayerAchievement = db.GetCollection<DbPlayerAchievement>("playerAchievement");
        colPlayerAuth = db.GetCollection<DbPlayerAuth>("playerAuth");
        colPlayerCurrency = db.GetCollection<DbPlayerCurrency>("playerCurrency");
        colPlayerStamina = db.GetCollection<DbPlayerStamina>("playerStamina");
        colPlayerFormation = db.GetCollection<DbPlayerFormation>("playerFormation");
        colPlayerUnlockItem = db.GetCollection<DbPlayerUnlockItem>("playerUnlockItem");
        colPlayerClearStage = db.GetCollection<DbPlayerClearStage>("playerClearStage");
        colPlayerBattle = db.GetCollection<DbPlayerBattle>("playerBattle");
    }

    protected override void DoGetAchievementList(string playerId, string loginToken, UnityAction<AchievementListResult> onFinish)
    {
        var result = new AchievementListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerAchievement.CloneList(colPlayerAchievement.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetAuthList(string playerId, string loginToken, UnityAction<AuthListResult> onFinish)
    {
        var result = new AuthListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerAuth.CloneList(colPlayerAuth.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetItemList(string playerId, string loginToken, UnityAction<ItemListResult> onFinish)
    {
        var result = new ItemListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerItem.CloneList(colPlayerItem.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetCurrencyList(string playerId, string loginToken, UnityAction<CurrencyListResult> onFinish)
    {
        var result = new CurrencyListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerCurrency.CloneList(GetPlayerCurrencies(playerId)));
        onFinish(result);
    }

    protected List<DbPlayerCurrency> GetPlayerCurrencies(string playerId)
    {
        List<DbPlayerCurrency> result = new List<DbPlayerCurrency>();
        foreach (var id in GameInstance.GameDatabase.Currencies.Keys)
        {
            result.Add(GetCurrency(playerId, id));
        }
        return result;
    }

    protected override void DoGetStaminaList(string playerId, string loginToken, UnityAction<StaminaListResult> onFinish)
    {
        var result = new StaminaListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerStamina.CloneList(GetPlayerStaminas(playerId)));
        onFinish(result);
    }

    protected List<DbPlayerStamina> GetPlayerStaminas(string playerId)
    {
        List<DbPlayerStamina> result = new List<DbPlayerStamina>();
        foreach (var id in GameInstance.GameDatabase.Staminas.Keys)
        {
            result.Add(GetStamina(playerId, id));
        }
        return result;
    }

    protected override void DoGetFormationList(string playerId, string loginToken, UnityAction<FormationListResult> onFinish)
    {
        var result = new FormationListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerFormation.CloneList(colPlayerFormation.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetUnlockItemList(string playerId, string loginToken, UnityAction<UnlockItemListResult> onFinish)
    {
        var result = new UnlockItemListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerUnlockItem.CloneList(colPlayerUnlockItem.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetClearStageList(string playerId, string loginToken, UnityAction<ClearStageListResult> onFinish)
    {
        var result = new ClearStageListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerClearStage.CloneList(colPlayerClearStage.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetServiceTime(UnityAction<ServiceTimeResult> onFinish)
    {
        var result = new ServiceTimeResult();
        result.serviceTime = Timestamp;
        onFinish(result);
    }

    protected override void DoGetChatMessages(string playerId, string loginToken, long lastTime, UnityAction<ChatMessageListResult> onFinish)
    {
        var result = new ChatMessageListResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }

    protected override void DoEnterChatMessage(string playerId, string loginToken, bool isClanChat, string message, UnityAction<GameServiceResult> onFinish)
    {
        var result = new GameServiceResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }
}
