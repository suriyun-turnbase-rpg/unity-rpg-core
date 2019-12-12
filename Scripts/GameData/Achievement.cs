using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementType : byte
{
    totalClearStage,
    totalClearStageRating,
    countLevelUpCharacter,
    countLevelUpEquipment,
    countEvolveCharacter,
    countEvolveEquipment,
    countRevive,
    countUseHelper,
    countWinStage,
    countWinDuel,
    countSellCharacter,
    countSellEquipment,
}

public class Achievement : BaseGameData
{
    public AchievementType type;
    public int targetAmount;

    public virtual string ToJson()
    {
        return "{\"type\":" + type + "," +
            "\"targetAmount\":" + targetAmount + "}";
    }
}
