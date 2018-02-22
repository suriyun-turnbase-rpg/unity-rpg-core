using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillCastAnimationData : AnimationData
{
    public CharacterEffectData castEffects;

    public static CharacterEffectData GetCastEffects(BaseSkillCastAnimationData skillCastAnimation)
    {
        return skillCastAnimation == null ? null : skillCastAnimation.castEffects;
    }
}
