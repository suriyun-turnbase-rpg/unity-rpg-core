[System.Serializable]
public class FakePlayer
{
    public string profileName;
    public int level;
    public CharacterItem mainCharacter;
    public int mainCharacterLevel;

    public int GetExp()
    {
        var exp = 1;
        var gameDb = GameInstance.GameDatabase;
        for (var i = 0; i < level; ++i)
        {
            exp += gameDb.playerExpTable.Calculate(i + 1, gameDb.playerMaxLevel);
        }
        return exp;
    }

    public int GetMainCharacterExp()
    {
        if (mainCharacter == null)
            return 0;
        var exp = 1;
        var itemTier = mainCharacter.itemTier;
        for (var i = 0; i < level; ++i)
        {
            exp += itemTier.expTable.Calculate(i + 1, itemTier.maxLevel);
        }
        return exp;
    }
}
