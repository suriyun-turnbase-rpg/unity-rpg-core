public abstract class BaseSkillCastAnimationData : AnimationData
{
    public CharacterEffectData castEffects;
}

public static class BaseSkillCastAnimationDataExtension
{
    public static CharacterEffectData GetCastEffects(this BaseSkillCastAnimationData skillCastAnimation)
    {
        return skillCastAnimation == null ? null : skillCastAnimation.castEffects;
    }
}
