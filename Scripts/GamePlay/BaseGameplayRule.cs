using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameplayRule : ScriptableObject
{
    public virtual float GetDamage(
        Elemental attackerElemental,
        Elemental defenderElemental,
        CalculationAttributes attackerAttributes,
        CalculationAttributes defenderAttributes,
        float pAtkRate = 1f,
        float mAtkRate = 1f,
        int hitCount = 1,
        int fixDamage = 0)
    {
        if (hitCount <= 0)
            hitCount = 1;

        var gameDb = GameInstance.GameDatabase;
        var pDmg = attackerAttributes.pAtk - defenderAttributes.pDef;
#if !NO_MAGIC_STATS
        var mDmg = attackerAttributes.mAtk - defenderAttributes.mDef;
#endif
        if (pDmg < 0)
            pDmg = 0;
#if !NO_MAGIC_STATS
        if (mDmg < 0)
            mDmg = 0;
#endif
        var totalDmg = pDmg;
#if !NO_MAGIC_STATS
        totalDmg += mDmg;
#endif
        // Increase / Decrease damage by effectiveness
        var effectiveness = 1f;
        if (attackerElemental != null && attackerElemental.CacheElementEffectiveness.TryGetValue(defenderElemental, out effectiveness))
            totalDmg *= effectiveness;
        totalDmg += Mathf.CeilToInt(totalDmg * Random.Range(gameDb.minAtkVaryRate, gameDb.maxAtkVaryRate)) + fixDamage;
        return totalDmg;
    }

    public virtual bool IsCrit(CalculationAttributes attackerAttributes, CalculationAttributes defenderAttributes)
    {
        return Random.value <= attackerAttributes.critChance;
    }

    public virtual float GetCritDamage(CalculationAttributes attackerAttributes, CalculationAttributes defenderAttributes, float damage)
    {
        return damage * attackerAttributes.critDamageRate;
    }

    public virtual bool IsBlock(CalculationAttributes attackerAttributes, CalculationAttributes defenderAttributes)
    {
        return Random.value <= defenderAttributes.blockChance;
    }

    public virtual float GetBlockDamage(CalculationAttributes attributes, CalculationAttributes defenderAttributes, float damage)
    {
        return damage / defenderAttributes.blockDamageRate;
    }

    public virtual bool IsHit(CalculationAttributes attackerAttributes, CalculationAttributes defenderAttributes)
    {
#if !NO_EVADE_STATS
        var hitChance = 1f;
        if (attackerAttributes.acc > 0 && defenderAttributes.eva > 0)
            hitChance = attackerAttributes.acc / defenderAttributes.eva;
        return hitChance < 0 || Random.value > hitChance;
#else
        return true;
#endif
    }
}
