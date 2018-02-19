using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData : ScriptableObject
{
    [Tooltip("0 = No animation")]
    [Range(0, 1000)]
    public int animationActionState = 0;
    public float animationDuration = 0;

    public static int GetAnimationActionState(AnimationData attackAnimation)
    {
        return attackAnimation == null ? 0 : attackAnimation.animationActionState;
    }

    public static float GetAnimationDuration(AnimationData attackAnimation)
    {
        return attackAnimation == null ? 0f : attackAnimation.animationDuration;
    }
}
