using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : MonoBehaviour, ILevel
{
    public Text textLevel;
    public Text textCollectExp;
    public Text textNextExp;
    public Text textCollectPerNextExp;
    public Text textRequireExp;
    public Text textExpPercent;
    public Image imageExpGage;
    public Text textIncreasingExp;
    public Text textIncreasedExpPercent;
    public Image imageIncreasedExpGage;
    public bool showMaxLevelInTextLevel;

    public int level;
    public int maxLevel;
    public int collectExp;
    public int increasingExp;
    public int nextExp;

    public int Level { get { return level; } }
    public int MaxLevel { get { return maxLevel; } }
    public int CollectExp { get { return collectExp; } }
    public int IncreasingExp { get { return increasingExp; } }
    public int NextExp { get { return nextExp; } }

    // Options
    public bool useFormatForInfo;

    void Update()
    {
        var rate = (float)(CollectExp - IncreasingExp) / (float)NextExp;
        var rateWithIncreasingExp = (float)CollectExp / (float)NextExp;
        var isReachMaxLevel = false;
        if (Level == MaxLevel)
        {
            isReachMaxLevel = true;
            rate = 1;
            rateWithIncreasingExp = 1;
        }

        if (textLevel != null)
        {
            textLevel.text = useFormatForInfo ? LanguageManager.FormatInfo(GameText.TITLE_LEVEL, Level) : Level.ToString("N0");
            if (showMaxLevelInTextLevel)
                textLevel.text += "/" + MaxLevel.ToString("N0");
        }

        if (textCollectExp != null)
        {
            textCollectExp.text = useFormatForInfo ? LanguageManager.FormatInfo(GameText.TITLE_COLLECT_EXP, (CollectExp - IncreasingExp)) : (CollectExp - IncreasingExp).ToString("N0");
            if (isReachMaxLevel)
                textCollectExp.text = "0";
        }

        if (textNextExp != null)
        {
            textNextExp.text = useFormatForInfo ? LanguageManager.FormatInfo(GameText.TITLE_NEXT_EXP, NextExp) : NextExp.ToString("N0");
            if (isReachMaxLevel)
                textNextExp.text = "0";
        }

        if (textCollectPerNextExp != null)
        {
            textCollectPerNextExp.text = (CollectExp - IncreasingExp).ToString("N0") + "/" + NextExp.ToString("N0");
            if (isReachMaxLevel)
                textCollectPerNextExp.text = LanguageManager.GetText(GameText.TITLE_EXP_MAX);
        }

        if (textRequireExp != null)
        {
            textRequireExp.text = useFormatForInfo ? LanguageManager.FormatInfo(GameText.TITLE_REQUIRE_EXP, this.RequireExp()) : this.RequireExp().ToString("N0");
            if (isReachMaxLevel)
                textRequireExp.text = "0";
        }

        if (textExpPercent != null)
            textExpPercent.text = (rate * 100).ToString("N2") + "%";

        if (imageExpGage != null)
            imageExpGage.fillAmount = rate;

        if (textIncreasingExp != null)
            textIncreasingExp.text = "+" + IncreasingExp.ToString("N0");

        if (textIncreasedExpPercent != null)
            textIncreasedExpPercent.text = (rateWithIncreasingExp * 100).ToString("N2") + "%";

        if (imageIncreasedExpGage != null)
            imageIncreasedExpGage.fillAmount = rateWithIncreasingExp;
    }
}
