using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementType : byte
{
    TotalClearStage,
    TotalClearStageRating,
    CountLevelUpCharacter,
    CountLevelUpEquipment,
    CountEvolveCharacter,
    CountEvolveEquipment,
    CountRevive,
    CountUseHelper,
    CountWinStage,
    CountWinDuel
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
