using UnityEngine;

public abstract class BaseAttackAnimationData : AnimationData
{
    [Range(0f, 1f)]
    public float hitDurationRate;
    public BaseDamage damage;
}

public static class BaseAttackAnimationDataExtension
{
    public static float GetHitDuration(this BaseAttackAnimationData attackAnimation)
    {
        return attackAnimation == null ? 0f : attackAnimation.hitDurationRate * attackAnimation.AnimationDuration;
    }

    public static BaseDamage GetDamage(this BaseAttackAnimationData attackAnimation)
    {
        return attackAnimation == null ? null : attackAnimation.damage;
    }
}