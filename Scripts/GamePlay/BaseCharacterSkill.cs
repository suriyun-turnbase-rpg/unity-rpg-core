public abstract class BaseCharacterSkill
{
    public int Level { get; protected set; }
    public BaseSkill Skill { get; protected set; }
    public string Id { get { return Skill.Id; } }
    public int MpCost { get { return Skill.GetMpCost(Level); } }

    public BaseCharacterSkill(int level, BaseSkill skill)
    {
        Level = level;
        Skill = skill;
    }

    public virtual void OnUseSkill(BaseCharacterEntity characterEntity)
    {
        characterEntity.Mp -= MpCost;
    }

    public virtual bool CanUse(BaseCharacterEntity characterEntity)
    {
        return characterEntity.Mp >= MpCost;
    }

    public abstract float GetCoolDownDurationRate();
    public abstract float GetCoolDownDuration();
}
