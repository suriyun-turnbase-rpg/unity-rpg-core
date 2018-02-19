using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackAnimationData : AnimationData
{
    public float hitDuration = 0;
    public BaseDamage damage;

    public static float GetHitDuration(BaseAttackAnimationData attackAnimation)
    {
        return attackAnimation == null ? 0f : attackAnimation.hitDuration;
    }

    public static BaseDamage GetDamage(BaseAttackAnimationData attackAnimation)
    {
        return attackAnimation == null ? null : attackAnimation.damage;
    }
}
