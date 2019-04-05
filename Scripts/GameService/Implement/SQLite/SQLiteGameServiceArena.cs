using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mono.Data.Sqlite;

public partial class SQLiteGameService
{
    protected override void DoArenaGetOpponentList(string playerId, string loginToken, UnityAction<FriendListResult> onFinish)
    {
        var result = new FriendListResult();
        var gameDb = GameInstance.GameDatabase;
        foreach (var fakePlayer in gameDb.fakePlayers)
        {
            if (fakePlayer.level <= 0 || fakePlayer.mainCharacter == null || fakePlayer.mainCharacterLevel <= 0)
                continue;
            result.list.Add(fakePlayer.MakePlayer());
        }
        onFinish(result);
    }

    protected override void DoStartDuel(string playerId, string loginToken, string targetPlayerId, UnityAction<StartDuelResult> onFinish)
    {
        var result = new StartDuelResult();
        var gameDb = GameInstance.GameDatabase;
        var player = GetPlayerByLoginToken(playerId, loginToken);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (!gameDb.FakePlayers.ContainsKey(targetPlayerId))
            result.error = GameServiceErrorCode.INVALID_PLAYER_DATA;
        else
        {
            var fakePlayer = gameDb.FakePlayers[targetPlayerId];
            ExecuteNonQuery(@"DELETE FROM playerBattle WHERE playerId=@playerId AND battleResult=@battleResult AND battleType=@battleType",
                new SqliteParameter("@playerId", playerId),
                new SqliteParameter("@battleResult", (byte)EBattleResult.None),
                new SqliteParameter("@battleType", (byte)EBattleType.Arena));
            var arenaStaminaTable = gameDb.arenaStamina;
            // Require stamina for arena always = 1
            if (!DecreasePlayerStamina(player, arenaStaminaTable, 1))
                result.error = GameServiceErrorCode.NOT_ENOUGH_STAGE_STAMINA;
            else
            {
                var playerBattle = new PlayerBattle();
                playerBattle.Id = System.Guid.NewGuid().ToString();
                playerBattle.PlayerId = playerId;
                playerBattle.DataId = targetPlayerId;
                playerBattle.Session = System.Guid.NewGuid().ToString();
                playerBattle.BattleResult = (byte)EBattleResult.None;
                playerBattle.BattleType = (byte)EBattleType.Arena;
                ExecuteNonQuery(@"INSERT INTO playerBattle (id, playerId, dataId, session, battleResult, rating, battleType) VALUES (@id, @playerId, @dataId, @session, @battleResult, @rating, @battleType)",
                    new SqliteParameter("@id", playerBattle.Id),
                    new SqliteParameter("@playerId", playerBattle.PlayerId),
                    new SqliteParameter("@dataId", playerBattle.DataId),
                    new SqliteParameter("@session", playerBattle.Session),
                    new SqliteParameter("@battleResult", playerBattle.BattleResult),
                    new SqliteParameter("@rating", playerBattle.Rating),
                    new SqliteParameter("@battleType", playerBattle.BattleType));

                var stamina = GetStamina(player.Id, arenaStaminaTable.id);
                result.stamina = stamina;
                result.session = playerBattle.Session;
                
                // Opponent characters
                foreach (var arenaCharacter in fakePlayer.arenaCharacters)
                {
                    result.opponentCharacters.Add(arenaCharacter.MakeAsItem());
                }
            }
        }
        onFinish(result);
    }

    protected override void DoFinishDuel(string playerId, string loginToken, string session, EBattleResult battleResult, int deadCharacters, UnityAction<FinishDuelResult> onFinish)
    {
        var result = new FinishDuelResult();
        var gameDb = GameInstance.GameDatabase;
        var player = GetPlayerByLoginToken(playerId, loginToken);
        var battle = GetPlayerBattleBySession(playerId, session);
        if (player == null)
            result.error = GameServiceErrorCode.INVALID_LOGIN_TOKEN;
        else if (battle == null)
            result.error = GameServiceErrorCode.INVALID_BATTLE_SESSION;
        else
        {
            var rating = 0;
            battle.BattleResult = (byte)battleResult;
            if (battleResult == EBattleResult.Win)
            {
                rating = 3 - deadCharacters;
                if (rating <= 0)
                    rating = 1;
            }
            battle.Rating = rating;
            result.rating = rating;
            ExecuteNonQuery(@"UPDATE playerBattle SET battleResult=@battleResult, rating=@rating WHERE id=@id",
                new SqliteParameter("@battleResult", battle.BattleResult),
                new SqliteParameter("@rating", battle.Rating),
                new SqliteParameter("@id", battle.Id));
            if (battleResult == EBattleResult.Win)
            {
                // Increase arena score
                var oldArenaScore = player.ArenaScore;
                var oldArenaLevel = player.ArenaLevel;
                result.updateScore = gameDb.arenaWinScoreIncrease;
                player.ArenaScore += gameDb.arenaWinScoreIncrease;
                ExecuteNonQuery(@"UPDATE player SET arenaScore=@arenaScore WHERE id=@playerId",
                    new SqliteParameter("@arenaScore", player.ArenaScore),
                    new SqliteParameter("@playerId", playerId));
                result.player = player;

                // Arena rank up, rewarding items
                if (player.ArenaLevel > oldArenaLevel && player.HighestArenaRankCurrentSeason < player.ArenaLevel)
                {
                    // Update highest rank
                    player.HighestArenaRankCurrentSeason = player.ArenaLevel;
                    if (player.HighestArenaRank < player.ArenaLevel)
                        player.HighestArenaRank = player.ArenaLevel;
                    ExecuteNonQuery(@"UPDATE player SET highestArenaRank=@highestArenaRank, highestArenaRankCurrentSeason=@highestArenaRankCurrentSeason WHERE id=@playerId",
                        new SqliteParameter("@highestArenaRank", player.HighestArenaRank),
                        new SqliteParameter("@highestArenaRankCurrentSeason", player.HighestArenaRankCurrentSeason),
                        new SqliteParameter("@playerId", playerId));

                    var arenaRank = gameDb.arenaRanks[oldArenaLevel];
                    // Soft currency
                    var softCurrency = GetCurrency(playerId, gameDb.softCurrency.id);
                    var rewardSoftCurrency = arenaRank.rewardSoftCurrency;
                    result.rewardSoftCurrency = rewardSoftCurrency;
                    softCurrency.Amount += rewardSoftCurrency;
                    ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                        new SqliteParameter("@amount", softCurrency.Amount),
                        new SqliteParameter("@id", softCurrency.Id));
                    result.updateCurrencies.Add(softCurrency);
                    // Hard currency
                    var hardCurrency = GetCurrency(playerId, gameDb.hardCurrency.id);
                    var rewardHardCurrency = arenaRank.rewardHardCurrency;
                    result.rewardHardCurrency = rewardHardCurrency;
                    hardCurrency.Amount += rewardHardCurrency;
                    ExecuteNonQuery(@"UPDATE playerCurrency SET amount=@amount WHERE id=@id",
                        new SqliteParameter("@amount", hardCurrency.Amount),
                        new SqliteParameter("@id", hardCurrency.Id));
                    result.updateCurrencies.Add(hardCurrency);
                    // Items
                    for (var i = 0; i < arenaRank.rewardItems.Length; ++i)
                    {
                        var rewardItem = arenaRank.rewardItems[i];
                        if (rewardItem == null || rewardItem.item == null)
                            continue;
                        var createItems = new List<PlayerItem>();
                        var updateItems = new List<PlayerItem>();
                        if (AddItems(player.Id, rewardItem.Id, rewardItem.amount, out createItems, out updateItems))
                        {
                            foreach (var createEntry in createItems)
                            {
                                createEntry.Id = System.Guid.NewGuid().ToString();
                                ExecuteNonQuery(@"INSERT INTO playerItem (id, playerId, dataId, amount, exp, equipItemId, equipPosition) VALUES (@id, @playerId, @dataId, @amount, @exp, @equipItemId, @equipPosition)",
                                    new SqliteParameter("@id", createEntry.Id),
                                    new SqliteParameter("@playerId", createEntry.PlayerId),
                                    new SqliteParameter("@dataId", createEntry.DataId),
                                    new SqliteParameter("@amount", createEntry.Amount),
                                    new SqliteParameter("@exp", createEntry.Exp),
                                    new SqliteParameter("@equipItemId", createEntry.EquipItemId),
                                    new SqliteParameter("@equipPosition", createEntry.EquipPosition));
                                result.rewardItems.Add(createEntry);
                                result.createItems.Add(createEntry);
                                HelperUnlockItem(player.Id, rewardItem.Id);
                            }
                            foreach (var updateEntry in updateItems)
                            {
                                ExecuteNonQuery(@"UPDATE playerItem SET playerId=@playerId, dataId=@dataId, amount=@amount, exp=@exp, equipItemId=@equipItemId, equipPosition=@equipPosition WHERE id=@id",
                                    new SqliteParameter("@playerId", updateEntry.PlayerId),
                                    new SqliteParameter("@dataId", updateEntry.DataId),
                                    new SqliteParameter("@amount", updateEntry.Amount),
                                    new SqliteParameter("@exp", updateEntry.Exp),
                                    new SqliteParameter("@equipItemId", updateEntry.EquipItemId),
                                    new SqliteParameter("@equipPosition", updateEntry.EquipPosition),
                                    new SqliteParameter("@id", updateEntry.Id));
                                result.rewardItems.Add(updateEntry);
                                result.updateItems.Add(updateEntry);
                            }
                        }
                        // End add item condition
                    }
                    // End reward items loop
                }
            }
            else
            {
                result.updateScore = -gameDb.arenaLoseScoreDecrease;
                player.ArenaScore -= gameDb.arenaLoseScoreDecrease;
                ExecuteNonQuery(@"UPDATE player SET arenaScore=@arenaScore WHERE id=@playerId",
                    new SqliteParameter("@arenaScore", player.ArenaScore),
                    new SqliteParameter("@playerId", playerId));
                result.player = player;
            }
        }
        onFinish(result); 
    }
}
