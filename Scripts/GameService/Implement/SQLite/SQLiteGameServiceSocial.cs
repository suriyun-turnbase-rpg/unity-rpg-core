using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class SQLiteGameService
{
    protected override void DoGetRandomPlayerList(string playerId, string loginToken, UnityAction<FriendListResult> onFinish)
    {
        // Random players from fake players list
        var result = new FriendListResult();
        var gameDb = GameInstance.GameDatabase;
        foreach (var fakePlayer in gameDb.fakePlayers)
        {
            if (fakePlayer.level <= 0 || fakePlayer.mainCharacter == null || fakePlayer.mainCharacterLevel <= 0)
                continue;
            var entry = new Player();
            entry.ProfileName = fakePlayer.profileName;
            entry.Exp = fakePlayer.GetExp();
            entry.MainCharacter = fakePlayer.mainCharacter.Id;
            entry.MainCharacterExp = fakePlayer.GetMainCharacterExp();
            result.list.Add(entry);
        }
        onFinish(result);
    }

    protected override void DoGetFriendList(string playerId, string loginToken, UnityAction<FriendListResult> onFinish)
    {
        // Return empty, this service (offline service) is not support it
        var result = new FriendListResult();
        onFinish(result);
    }

    protected override void DoGetFriendRequestList(string playerId, string loginToken, UnityAction<FriendListResult> onFinish)
    {
        // Return empty, this service (offline service) is not support it
        var result = new FriendListResult();
        onFinish(result);
    }

    protected override void DoFriendRequest(string playerId, string loginToken, string targetPlayerId, UnityAction<GameServiceResult> onFinish)
    {
        // Return fail, this service (offline service) is not support it
        var result = new GameServiceResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }

    protected override void DoFriendAccept(string playerId, string loginToken, string targetPlayerId, UnityAction<FriendListResult> onFinish)
    {
        // Return fail, this service (offline service) is not support it
        var result = new FriendListResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }

    protected override void DoFriendDecline(string playerId, string loginToken, string targetPlayerId, UnityAction<GameServiceResult> onFinish)
    {
        // Return fail, this service (offline service) is not support it
        var result = new GameServiceResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }

    protected override void DoFriendDelete(string playerId, string loginToken, string targetPlayerId, UnityAction<FriendListResult> onFinish)
    {
        // Return fail, this service (offline service) is not support it
        var result = new FriendListResult();
        result.error = GameServiceErrorCode.NOT_AVAILABLE;
        onFinish(result);
    }
}
