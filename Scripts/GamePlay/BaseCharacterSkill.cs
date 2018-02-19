using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterSkill
{
    public int Level { get; protected set; }
    public BaseSkill Skill { get; protected set; }
}
