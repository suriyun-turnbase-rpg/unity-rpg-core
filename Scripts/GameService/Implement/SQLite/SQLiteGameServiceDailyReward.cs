using UnityEngine.Events;

public partial class SQLiteGameService
{
    protected override void DoGetDailyRewardList(string playerId, string loginToken, string id, UnityAction<DailyRewardListResult> onFinish)
    {
        var result = new DailyRewardListResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }

    protected override void DoClaimDailyReward(string playerId, string loginToken, string id, UnityAction<GameServiceResult> onFinish)
    {
        var result = new GameServiceResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }
}
