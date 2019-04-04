using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class LiteDbGameService
{
    protected override void DoGetOpponentList(string playerId, string loginToken, UnityAction<FriendListResult> onFinish)
    {
        var result = new FriendListResult();
        onFinish(result);
    }

    protected override void DoStartDuel(string playerId, string loginToken, string targetPlayerId, UnityAction<StartDuelResult> onFinish)
    {

    }

    protected override void DoFinishDuel(string playerId, string loginToken, string session, ushort battleResult, int deadCharacters, UnityAction<FinishDuelResult> onFinish)
    {
        
    }
}
