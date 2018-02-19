using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterBuff
{
    public BaseCharacterEntity Giver { get; protected set; }
    public BaseCharacterEntity Receiver { get; protected set; }
    public int Level { get; protected set; }
    public BaseSkill Skill { get; protected set; }
    public int BuffIndex { get; protected set; }
    public string Id { get { return Skill.Id + "_" + BuffIndex; } }
    private List<BaseSkillBuff> buffs;
    public List<BaseSkillBuff> Buffs
    {
        get
        {
            if (buffs == null)
                buffs = Skill.GetBuffs();
            return buffs;
        }
    }
    public BaseSkillBuff Buff { get { return Buffs[BuffIndex]; } }
    public CalculationAttributes Attributes { get { return Buff.GetAttributes(Level); } }
    public float PAtkHealRate { get { return Buff.GetPAtkHealRate(Level); } }
    public float MAtkHealRate { get { return Buff.GetMAtkHealRate(Level); } }
}
