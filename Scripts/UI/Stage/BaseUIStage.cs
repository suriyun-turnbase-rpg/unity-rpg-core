using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BaseUIStage<TPreparation, TStage> : UIDataItem<TStage>
    where TPreparation : UIDataItem<TStage>
    where TStage : BaseStage
{
    [Header("General")]
    public Text textTitle;
    public Text textDescription;
    public Text textStageNumber;
    public Image imageIcon;
    public Text textRecommendBattlePoint;
    public UIStamina uiRequireStamina;
    public Text textRewardPlayerExp;
    public Text textRewardCharacterExp;
    public UIItemList uiRewardsItemList;
    public UIItemList uiEnemyItemList;
    [Header("Relates Elements")]
    public Button[] interactableButtonsWhenUnlocked;
    public GameObject[] activeObjectsWhenUnlocked;
    public GameObject[] inactiveObjectsWhenUnlocked;
    public UnityEvent eventSetStagePreparation;

    public override void Clear()
    {
        // Don't clear
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(TStage data)
    {
        if (textTitle != null)
            textTitle.text = data.title;

        if (textDescription != null)
            textDescription.text = data.description;

        if (textStageNumber != null)
            textStageNumber.text = data.stageNumber;

        if (imageIcon != null)
            imageIcon.sprite = data.icon;

        if (textRecommendBattlePoint != null)
            textRecommendBattlePoint.text = data.recommendBattlePoint.ToString("N0");

        if (uiRequireStamina != null)
        {
            var staminaData = PlayerStamina.StageStamina.Clone();
            if (!string.IsNullOrEmpty(data.requireCustomStamina) && PlayerStamina.HasStamina(data.requireCustomStamina))
                staminaData = PlayerStamina.GetStamina(data.requireCustomStamina).Clone();
            staminaData.SetAmount(data.requireStamina, 0);
            uiRequireStamina.SetData(staminaData);
        }

        if (textRewardPlayerExp != null)
            textRewardPlayerExp.text = data.rewardPlayerExp.ToString("N0");

        if (textRewardCharacterExp != null)
            textRewardCharacterExp.text = data.rewardCharacterExp.ToString("N0");

        if (uiRewardsItemList != null)
        {
            var list = data.GetRewardItems();
            uiRewardsItemList.SetListItems(list, (ui) => ui.displayStats = UIItem.DisplayStats.Hidden);
        }

        if (uiEnemyItemList != null)
        {
            var list = data.GetCharacters();
            list.SortLevel();
            uiEnemyItemList.SetListItems(list, (ui) => ui.displayStats = UIItem.DisplayStats.Level);
        }

        UpdateElementsWhenUnlocked();
    }

    public void UpdateElementsWhenUnlocked()
    {
        var isUnlocked = PlayerClearStage.IsUnlock(data) && GameInstance.AvailableStages.Contains(data.Id);
        foreach (var button in interactableButtonsWhenUnlocked)
        {
            button.interactable = isUnlocked;
        }
        foreach (var obj in activeObjectsWhenUnlocked)
        {
            obj.SetActive(isUnlocked);
        }
        foreach (var obj in inactiveObjectsWhenUnlocked)
        {
            obj.SetActive(!isUnlocked);
        }
    }

    public void SetStagePreparationData()
    {
        if (StagePreparation != null)
        {
            StagePreparation.data = data;
            eventSetStagePreparation.Invoke();
        }
    }

    public void ShowStagePreparation()
    {
        if (StagePreparation != null)
        {
            var historyManager = FindObjectOfType<UIHistoryManager>();
            if (historyManager)
                historyManager.Next(StagePreparation);
            else
                StagePreparation.Show();
        }
    }


    public TPreparation uiStagePreparation;
    public TPreparation StagePreparation
    {
        get { return uiStagePreparation; }
    }
}
