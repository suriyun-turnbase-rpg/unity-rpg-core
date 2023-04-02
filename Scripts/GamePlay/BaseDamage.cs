using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseDamage : MonoBehaviour
{
    private Transform tempTransform;
    public Transform TempTransform
    {
        get
        {
            if (tempTransform == null)
                tempTransform = GetComponent<Transform>();
            return tempTransform;
        }
    }
    private Rigidbody tempRigidbody;
    public Rigidbody TempRigidbody
    {
        get
        {
            if (tempRigidbody == null)
                tempRigidbody = GetComponent<Rigidbody>();
            return tempRigidbody;
        }
    }

    public abstract void Setup(
        BaseCharacterEntity attacker,
        BaseCharacterEntity target,
        int seed,
        float pAtkRate,
        float mAtkRate,
        int hitCount,
        int fixDamage,
        bool fromCounter,
        bool useSkillHitChance,
        float skillHitChance);
}
