using UnityEngine.Events;

public partial class LiteDbGameService
{
    protected override void DoGetUnlockIconList(string playerId, string loginToken, UnityAction<UnlockIconListResult> onFinish)
    {
        var result = new UnlockIconListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerUnlockIcon.CloneList(colPlayerUnlockIcon.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetUnlockFrameList(string playerId, string loginToken, UnityAction<UnlockFrameListResult> onFinish)
    {
        var result = new UnlockFrameListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerUnlockFrame.CloneList(colPlayerUnlockFrame.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetUnlockTitleList(string playerId, string loginToken, UnityAction<UnlockTitleListResult> onFinish)
    {
        var result = new UnlockTitleListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbPlayerUnlockTitle.CloneList(colPlayerUnlockTitle.Find(a => a.PlayerId == playerId)));
        onFinish(result);
    }

    protected override void DoGetClanUnlockIconList(string playerId, string loginToken, UnityAction<ClanUnlockIconListResult> onFinish)
    {
        var result = new ClanUnlockIconListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbClanUnlockIcon.CloneList(colClanUnlockIcon.Find(a => a.ClanId == player.ClanId)));
        onFinish(result);
    }

    protected override void DoGetClanUnlockFrameList(string playerId, string loginToken, UnityAction<ClanUnlockFrameListResult> onFinish)
    {
        var result = new ClanUnlockFrameListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbClanUnlockFrame.CloneList(colClanUnlockFrame.Find(a => a.ClanId == player.ClanId)));
        onFinish(result);
    }

    protected override void DoGetClanUnlockTitleList(string playerId, string loginToken, UnityAction<ClanUnlockTitleListResult> onFinish)
    {
        var result = new ClanUnlockTitleListResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else
            result.list.AddRange(DbClanUnlockTitle.CloneList(colClanUnlockTitle.Find(a => a.ClanId == player.ClanId)));
        onFinish(result);
    }

    protected override void DoSetPlayerIcon(string playerId, string loginToken, string iconDataId, UnityAction<GameServiceResult> onFinish)
    {
        var result = new GameServiceResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
        {
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        }
        else
        {
            player.IconId = iconDataId;
            colPlayer.Update(player);
        }
        onFinish(result);
    }

    protected override void DoSetPlayerFrame(string playerId, string loginToken, string frameDataId, UnityAction<GameServiceResult> onFinish)
    {
        var result = new GameServiceResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
        {
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        }
        else
        {
            player.FrameId = frameDataId;
            colPlayer.Update(player);
        }
        onFinish(result);
    }

    protected override void DoSetPlayerTitle(string playerId, string loginToken, string titleDataId, UnityAction<GameServiceResult> onFinish)
    {
        var result = new GameServiceResult();
        var player = colPlayer.FindOne(a => a.Id == playerId && a.LoginToken == loginToken);
        if (player == null)
        {
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        }
        else
        {
            player.TitleId = titleDataId;
            colPlayer.Update(player);
        }
        onFinish(result);
    }

    protected override void DoSetClanIcon(string playerId, string loginToken, string iconDataId, UnityAction<GameServiceResult> onFinish)
    {
        var result = new GameServiceResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }

    protected override void DoSetClanFrame(string playerId, string loginToken, string frameDataId, UnityAction<GameServiceResult> onFinish)
    {
        var result = new GameServiceResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }

    protected override void DoSetClanTitle(string playerId, string loginToken, string titleDataId, UnityAction<GameServiceResult> onFinish)
    {
        var result = new GameServiceResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }
}
