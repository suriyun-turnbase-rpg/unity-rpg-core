using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[Serializable]
public struct Int32Attribute
{
    public int minValue;
    public int maxValue;
    public float growth;

    public Int32Attribute Clone()
    {
        var result = new Int32Attribute();
        result.minValue = minValue;
        result.maxValue = maxValue;
        result.growth = growth;
        return result;
    }

    public int Calculate(int currentLevel, int maxLevel)
    {
        if (currentLevel <= 0)
            currentLevel = 1;
        if (maxLevel <= 0)
            maxLevel = 1;
        if (currentLevel == 1)
            return minValue;
        if (currentLevel == maxLevel)
            return maxValue;
        return minValue + Mathf.RoundToInt((maxValue - minValue) * Mathf.Pow((float)(currentLevel - 1) / (float)(maxLevel - 1), growth));
    }
    
    public static Int32Attribute operator *(Int32Attribute a, float multiplier)
    {
        var result = a.Clone();
        result.minValue = Mathf.RoundToInt(a.minValue * multiplier);
        result.maxValue = Mathf.RoundToInt(a.maxValue * multiplier);
        return result;
    }

    public string ToJson()
    {
        return "{\"minValue\":" + minValue + "," +
            "\"maxValue\":" + maxValue + "," +
            "\"growth\":" + growth.ToString(new CultureInfo("en-US", false)) + "}";
    }
}

[Serializable]
public struct SingleAttribute
{
    public float minValue;
    public float maxValue;
    public float growth;

    public SingleAttribute Clone()
    {
        var result = new SingleAttribute();
        result.minValue = minValue;
        result.maxValue = maxValue;
        result.growth = growth;
        return result;
    }

    public float Calculate(int currentLevel, int maxLevel)
    {
        if (currentLevel <= 0)
            currentLevel = 1;
        if (maxLevel <= 0)
            maxLevel = 1;
        if (currentLevel == 1)
            return minValue;
        if (currentLevel == maxLevel)
            return maxValue;
        return minValue + ((maxValue - minValue) * Mathf.Pow((float)(currentLevel - 1) / (float)(maxLevel - 1), growth));
    }

    public static SingleAttribute operator *(SingleAttribute a, float multiplier)
    {
        var result = a.Clone();
        result.minValue = a.minValue * multiplier;
        result.maxValue = a.maxValue * multiplier;
        return result;
    }

    public string ToJson()
    {
        return "{\"minValue\":" + minValue + "," +
            "\"maxValue\":" + maxValue + "," +
            "\"growth\":" + growth.ToString(new CultureInfo("en-US", false)) + "}";
    }
}

[Serializable]
public struct Attributes
{
    [Tooltip("Max Hp, When battle if character's Hp = 0, The character will die")]
    public Int32Attribute hp;
    [Tooltip("P.Attack (P stands for physical), This will minus to pDef to calculate damage")]
    public Int32Attribute pAtk;
    [Tooltip("P.Defend (P stands for physical), pAtk will minus to this to calculate damage")]
    public Int32Attribute pDef;
#if !NO_MAGIC_STATS
    [Tooltip("M.Attack (M stands for magical), This will minus to mDef to calculate damage")]
    public Int32Attribute mAtk;
    [Tooltip("M.Defend (M stands for magical), mAtk will minus to this to calculate damage")]
    public Int32Attribute mDef;
#endif
    [Tooltip("Speed, Character with higher speed will have more chance to attack")]
    public Int32Attribute spd;
#if !NO_EVADE_STATS
    [Tooltip("Evasion, Character with higher evasion will have more chance to avoid damage from character with lower accuracy")]
    public Int32Attribute eva;
    [Tooltip("Accuracy, Character with higher accuracy will have more chance to take damage to character with lower evasion")]
    public Int32Attribute acc;
#endif
    [Header("Critical attributes")]
    [Tooltip("Chance to critical attack (Increase damage by `critDamageRate`), this min-max value should not over 1")]
    public SingleAttribute critChance;
    [Tooltip("Damage when critical attack = this * Damage")]
    public SingleAttribute critDamageRate;
    [Header("Block attributes")]
    [Tooltip("Chance to block (Reduce damage by `blockDamageRate`), this min-max value should not over 1")]
    public SingleAttribute blockChance;
    [Tooltip("Damage when block = this / Damage")]
    public SingleAttribute blockDamageRate;
    [Header("Resistance Attributes")]
    [Tooltip("Chance to prevent application of a nerf effect, this min-max value should not over 1")]
    public SingleAttribute resistanceChance;

    public Attributes Clone()
    {
        Attributes result = new Attributes();
        result.hp = hp.Clone();
        result.pAtk = pAtk.Clone();
        result.pDef = pDef.Clone();
#if !NO_MAGIC_STATS
        result.mAtk = mAtk.Clone();
        result.mDef = mDef.Clone();
#endif
        result.spd = spd.Clone();
#if !NO_EVADE_STATS
        result.eva = eva.Clone();
        result.acc = acc.Clone();
#endif
        result.critChance = critChance.Clone();
        result.critDamageRate = critDamageRate.Clone();
        result.blockChance = blockChance.Clone();
        result.blockDamageRate = blockDamageRate.Clone();
        result.resistanceChance = resistanceChance.Clone();
        return result;
    }

    public CalculatedAttributes CreateCalculationAttributes(int currentLevel, int maxLevel)
    {
        CalculatedAttributes result = new CalculatedAttributes();
        result.hp = hp.Calculate(currentLevel, maxLevel);
        result.pAtk = pAtk.Calculate(currentLevel, maxLevel);
        result.pDef = pDef.Calculate(currentLevel, maxLevel);
#if !NO_MAGIC_STATS
        result.mAtk = mAtk.Calculate(currentLevel, maxLevel);
        result.mDef = mDef.Calculate(currentLevel, maxLevel);
#endif
        result.spd = spd.Calculate(currentLevel, maxLevel);
#if !NO_EVADE_STATS
        result.eva = eva.Calculate(currentLevel, maxLevel);
        result.acc = acc.Calculate(currentLevel, maxLevel);
#endif
        result.critChance = critChance.Calculate(currentLevel, maxLevel);
        result.critDamageRate = critDamageRate.Calculate(currentLevel, maxLevel);
        result.blockChance = blockChance.Calculate(currentLevel, maxLevel);
        result.blockDamageRate = blockDamageRate.Calculate(currentLevel, maxLevel);
        result.resistanceChance = resistanceChance.Calculate(currentLevel, maxLevel);
        return result;
    }

    public Attributes CreateOverrideMaxLevelAttributes(int defaultMaxLevel, int newMaxLevel)
    {
        Attributes attributes = new Attributes();
        var hp = this.hp.Clone();
        hp.maxValue = this.hp.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.hp = hp;

        var pAtk = this.pAtk.Clone();
        pAtk.maxValue = this.pAtk.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.pAtk = pAtk;

        var pDef = this.pDef.Clone();
        pDef.maxValue = this.pDef.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.pDef = pDef;

#if !NO_MAGIC_STATS
        var mAtk = this.mAtk.Clone();
        mAtk.maxValue = this.mAtk.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.mAtk = mAtk;

        var mDef = this.mDef.Clone();
        mDef.maxValue = this.mDef.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.mDef = mDef;
#endif

        var spd = this.spd.Clone();
        spd.maxValue = this.spd.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.spd = spd;

#if !NO_EVADE_STATS
        var eva = this.eva.Clone();
        eva.maxValue = this.eva.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.eva = eva;

        var acc = this.acc.Clone();
        acc.maxValue = this.acc.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.acc = acc;
#endif

        var critChance = this.critChance.Clone();
        critChance.maxValue = this.critChance.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.critChance = critChance;

        var critDamageRate = this.critDamageRate.Clone();
        critDamageRate.maxValue = this.critDamageRate.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.critDamageRate = critDamageRate;

        var blockChance = this.blockChance.Clone();
        blockChance.maxValue = this.blockChance.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.blockChance = blockChance;

        var blockDamageRate = this.blockDamageRate.Clone();
        blockDamageRate.maxValue = this.blockDamageRate.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.blockDamageRate = blockDamageRate;

        var resistanceChance = this.resistanceChance.Clone();
        resistanceChance.maxValue = this.resistanceChance.Calculate(newMaxLevel, defaultMaxLevel);
        attributes.resistanceChance = resistanceChance;

        return attributes;
    }

    public static Attributes operator *(Attributes a, float b)
    {
        Attributes result = new Attributes();
        result.hp = a.hp * b;
        result.pAtk = a.pAtk * b;
        result.pDef = a.pDef * b;
#if !NO_MAGIC_STATS
        result.mAtk = a.mAtk * b;
        result.mDef = a.mDef * b;
#endif
        result.spd = a.spd * b;
#if !NO_EVADE_STATS
        result.eva = a.eva * b;
        result.acc = a.acc * b;
#endif
        result.critChance = a.critChance * b;
        result.critDamageRate = a.critDamageRate * b;
        result.blockChance = a.blockChance * b;
        result.blockDamageRate = a.blockDamageRate * b;
        result.resistanceChance = a.resistanceChance * b;
        return result;
    }
}

[Serializable]
public struct CalculatedAttributes
{
    [Header("Fix attributes")]
    [Tooltip("C.hp (C stands for Character) = C.hp + this")]
    public float hp;
    [Tooltip("C.pAtk (C stands for Character) = C.pAtk + this")]
    public float pAtk;
    [Tooltip("C.pDef (C stands for Character) = C.pDef + this")]
    public float pDef;
#if !NO_MAGIC_STATS
    [Tooltip("C.mAtk (C stands for Character) = C.mAtk + this")]
    public float mAtk;
    [Tooltip("C.mDef (C stands for Character) = C.mDef + this")]
    public float mDef;
#endif
    [Tooltip("C.Spd (C stands for Character) = C.Spd + this")]
    public float spd;
#if !NO_EVADE_STATS
    [Tooltip("C.Eva (C stands for Character) = C.Eva + this")]
    public float eva;
    [Tooltip("C.Acc (C stands for Character) = C.Acc + this")]
    public float acc;
#endif
    [Header("Rate attributes")]
    [Tooltip("C.hp (C stands for Character) = C.hp + (this * C.hp)")]
    public float hpRate;
    [Tooltip("C.pAtk (C stands for Character) = C.pAtk + (this * C.pAtk)")]
    public float pAtkRate;
    [Tooltip("C.pDef (C stands for Character) = C.pDef + (this * C.pDef)")]
    public float pDefRate;
#if !NO_MAGIC_STATS
    [Tooltip("C.mAtk (C stands for Character) = C.mAtk + (this * C.mAtk)")]
    public float mAtkRate;
    [Tooltip("C.mDef (C stands for Character) = C.mDef + (this * C.mDef)")]
    public float mDefRate;
#endif
    [Tooltip("C.Spd (C stands for Character) = C.Spd + (this * C.Spd)")]
    public float spdRate;
#if !NO_EVADE_STATS
    [Tooltip("C.Eva (C stands for Character) = C.Eva + (this * C.Eva)")]
    public float evaRate;
    [Tooltip("C.Acc (C stands for Character) = C.Acc + (this * C.Acc)")]
    public float accRate;
#endif
    [Header("Critical attributes")]
    [Range(0f, 1f)]
    [Tooltip("Chance to critical attack (Increase damage by `critDamageRate`)")]
    public float critChance;
    [Range(1f, 100f)]
    [Tooltip("Damage when critical attack = this * Damage")]
    public float critDamageRate;
    [Header("Block attributes")]
    [Range(0f, 1f)]
    [Tooltip("Chance to block (Reduce damage by `blockDamageRate`)")]
    public float blockChance;
    [Range(1f, 100f)]
    [Tooltip("Damage when block = this / Damage")]
    public float blockDamageRate;
    [Header("Resistance Attributes")]
    [Tooltip("Chance to prevent application of a harmful effect")]
    [Range(0f, 1f)]
    public float resistanceChance;

    public CalculatedAttributes Clone()
    {
        CalculatedAttributes result = new CalculatedAttributes();
        result.hp = hp;
        result.pAtk = pAtk;
        result.pDef = pDef;
#if !NO_MAGIC_STATS
        result.mAtk = mAtk;
        result.mDef = mDef;
#endif
        result.spd = spd;
#if !NO_EVADE_STATS
        result.eva = eva;
        result.acc = acc;
#endif

        result.hpRate = hpRate;
        result.pAtkRate = pAtkRate;
        result.pDefRate = pDefRate;
#if !NO_MAGIC_STATS
        result.mAtkRate = mAtkRate;
        result.mDefRate = mDefRate;
#endif
        result.spdRate = spdRate;
#if !NO_EVADE_STATS
        result.evaRate = evaRate;
        result.accRate = accRate;
#endif

        result.critChance = critChance;
        result.critDamageRate = critDamageRate;

        result.blockChance = blockChance;
        result.blockDamageRate = blockDamageRate;

        result.resistanceChance = resistanceChance;

        return result;
    }

#region Calculating between CalculationAttributes and CalculationAttributes
    public static CalculatedAttributes operator +(CalculatedAttributes a, CalculatedAttributes b)
    {
        CalculatedAttributes result = a.Clone();
        result.hp += b.hp;
        result.pAtk += b.pAtk;
        result.pDef += b.pDef;
#if !NO_MAGIC_STATS
        result.mAtk += b.mAtk;
        result.mDef += b.mDef;
#endif
        result.spd += b.spd;
#if !NO_EVADE_STATS
        result.eva += b.eva;
        result.acc += b.acc;
#endif

        result.hpRate += b.hpRate;
        result.pAtkRate += b.pAtkRate;
        result.pDefRate += b.pDefRate;
#if !NO_MAGIC_STATS
        result.mAtkRate += b.mAtkRate;
        result.mDefRate += b.mDefRate;
#endif
        result.spdRate += b.spdRate;
#if !NO_EVADE_STATS
        result.evaRate += b.evaRate;
        result.accRate += b.accRate;
#endif

        result.critChance += b.critChance;
        result.critDamageRate += b.critDamageRate;

        result.blockChance += b.blockChance;
        result.blockDamageRate += b.blockDamageRate;

        result.resistanceChance += b.resistanceChance;

        return result;
    }

    public static CalculatedAttributes operator -(CalculatedAttributes a, CalculatedAttributes b)
    {
        CalculatedAttributes result = a.Clone();
        result.hp -= b.hp;
        result.pAtk -= b.pAtk;
        result.pDef -= b.pDef;
#if !NO_MAGIC_STATS
        result.mAtk -= b.mAtk;
        result.mDef -= b.mDef;
#endif
        result.spd -= b.spd;
#if !NO_EVADE_STATS
        result.eva -= b.eva;
        result.acc -= b.acc;
#endif

        result.hpRate -= b.hpRate;
        result.pAtkRate -= b.pAtkRate;
        result.pDefRate -= b.pDefRate;
#if !NO_MAGIC_STATS
        result.mAtkRate -= b.mAtkRate;
        result.mDefRate -= b.mDefRate;
#endif
        result.spdRate -= b.spdRate;
#if !NO_EVADE_STATS
        result.evaRate -= b.evaRate;
        result.accRate -= b.accRate;
#endif

        result.critChance -= b.critChance;
        result.critDamageRate -= b.critDamageRate;

        result.blockChance -= b.blockChance;
        result.blockDamageRate -= b.blockDamageRate;

        result.resistanceChance -= b.resistanceChance;

        return result;
    }

    public static CalculatedAttributes operator *(CalculatedAttributes a, float b)
    {
        CalculatedAttributes result = new CalculatedAttributes();
        result.hp = Mathf.CeilToInt(a.hp * b);
        result.pAtk = Mathf.CeilToInt(a.pAtk * b);
        result.pDef = Mathf.CeilToInt(a.pDef * b);
#if !NO_MAGIC_STATS
        result.mAtk = Mathf.CeilToInt(a.mAtk * b);
        result.mDef = Mathf.CeilToInt(a.mDef * b);
#endif
        result.spd = Mathf.CeilToInt(a.spd * b);
#if !NO_EVADE_STATS
        result.eva = Mathf.CeilToInt(a.eva * b);
        result.acc = Mathf.CeilToInt(a.acc * b);
#endif

        result.hpRate = a.hpRate * b;
        result.pAtkRate = a.pAtkRate * b;
        result.pDefRate = a.pDefRate * b;
#if !NO_MAGIC_STATS
        result.mAtkRate = a.mAtkRate * b;
        result.mDefRate = a.mDefRate * b;
#endif
        result.spdRate = a.spdRate * b;
#if !NO_EVADE_STATS
        result.evaRate = a.evaRate * b;
        result.accRate = a.accRate * b;
#endif

        result.critChance = a.critChance * b;
        result.critDamageRate = a.critDamageRate * b;

        result.blockChance = a.blockChance * b;
        result.blockDamageRate = a.blockDamageRate * b;

        result.resistanceChance = a.resistanceChance * b;

        return result;
    }
#endregion

    public string GetDescription(CalculatedAttributes bonusAttributes)
    {
        var result = "";

        if (hp != 0 || bonusAttributes.hp != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_HP, hp, bonusAttributes.hp);
            result += "\n";
        }
        if (pAtk != 0 || bonusAttributes.pAtk != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_PATK, pAtk, bonusAttributes.pAtk);
            result += "\n";
        }
        if (pDef != 0 || bonusAttributes.pDef != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_PDEF, pDef, bonusAttributes.pDef);
            result += "\n";
        }
#if !NO_MAGIC_STATS
        if (mAtk != 0 || bonusAttributes.mAtk != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_MATK, mAtk, bonusAttributes.mAtk);
            result += "\n";
        }
        if (mDef != 0 || bonusAttributes.mDef != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_MDEF, mDef, bonusAttributes.mDef);
            result += "\n";
        }
#endif
        if (spd != 0 || bonusAttributes.spd != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_SPD, spd, bonusAttributes.spd);
            result += "\n";
        }
#if !NO_EVADE_STATS
        if (eva != 0 || bonusAttributes.eva != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_EVA, eva, bonusAttributes.eva);
            result += "\n";
        }
        if (acc != 0 || bonusAttributes.acc != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_ACC, acc, bonusAttributes.acc);
            result += "\n";
        }
#endif
        if (hpRate != 0 || bonusAttributes.hpRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_HP_RATE, hpRate, bonusAttributes.hpRate, true);
            result += "\n";
        }
        if (pAtkRate != 0 || bonusAttributes.pAtkRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_PATK_RATE, pAtkRate, bonusAttributes.pAtkRate, true);
            result += "\n";
        }
        if (pDefRate != 0 || bonusAttributes.pDefRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_PDEF_RATE, pDefRate, bonusAttributes.pDefRate, true);
            result += "\n";
        }
#if !NO_MAGIC_STATS
        if (mAtkRate != 0 || bonusAttributes.mAtkRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_MATK_RATE, mAtkRate, bonusAttributes.mAtkRate, true);
            result += "\n";
        }
        if (mDefRate != 0 || bonusAttributes.mDefRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_MDEF_RATE, mDefRate, bonusAttributes.mDefRate, true);
            result += "\n";
        }
#endif
        if (spdRate != 0 || bonusAttributes.spdRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_SPD_RATE, spdRate, bonusAttributes.spdRate, true);
            result += "\n";
        }
#if !NO_EVADE_STATS
        if (evaRate != 0 || bonusAttributes.evaRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_EVA_RATE, evaRate, bonusAttributes.evaRate, true);
            result += "\n";
        }
        if (accRate != 0 || bonusAttributes.accRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_ACC_RATE, accRate, bonusAttributes.accRate, true);
            result += "\n";
        }
#endif
        if (critChance != 0 || bonusAttributes.critChance != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_CRIT_CHANCE, critChance, bonusAttributes.critChance, true);
            result += "\n";
        }
        if (critDamageRate != 0 || bonusAttributes.critDamageRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_CRIT_DAMAGE_RATE, critDamageRate, bonusAttributes.critDamageRate, true);
            result += "\n";
        }
        if (blockChance != 0 || bonusAttributes.blockChance != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_BLOCK_CHANCE, blockChance, bonusAttributes.blockChance, true);
            result += "\n";
        }
        if (blockDamageRate != 0 || bonusAttributes.blockDamageRate != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_BLOCK_DAMAGE_RATE, blockDamageRate, bonusAttributes.blockDamageRate, true);
        }
        if (resistanceChance != 0 || bonusAttributes.resistanceChance != 0)
        {
            result += LanguageManager.FormatAttribute(GameText.TITLE_ATTRIBUTE_RESISTANCE_CHANCE, resistanceChance, bonusAttributes.resistanceChance, true);
        }
        return result;
    }
}
