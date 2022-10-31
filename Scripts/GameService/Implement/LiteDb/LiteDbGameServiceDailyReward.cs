using UnityEngine.Events;

public partial class LiteDbGameService
{
    protected override void DoGetAllDailyRewardList(string playerId, string loginToken, UnityAction<AllDailyRewardListResult> onFinish)
    {
        var result = new AllDailyRewardListResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }

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
