using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGamePackage : UIDataItem<InGamePackage>
{
    public Text textTitle;
    public Text textDescription;
    public Image imageIcon;
    public Image imageIcon2;
    public Image imageIcon3;
    public Image imageHighlight;
    public UICurrency uiCurrency;

    public override void Clear()
    {
        SetupInfo(null);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(InGamePackage data)
    {
        if (textTitle != null)
            textTitle.text = data == null ? "" : data.Title;

        if (textDescription != null)
            textDescription.text = data == null ? "" : data.Description;

        if (imageIcon != null)
        {
            imageIcon.sprite = data == null ? null : data.icon;
            imageIcon.preserveAspect = true;
        }

        if (imageIcon2 != null)
        {
            imageIcon2.sprite = data == null ? null : data.icon2;
            imageIcon2.preserveAspect = true;
        }

        if (imageIcon3 != null)
        {
            imageIcon3.sprite = data == null ? null : data.icon3;
            imageIcon3.preserveAspect = true;
        }

        if (imageHighlight != null)
        {
            imageHighlight.sprite = data.highlight;
            imageHighlight.preserveAspect = true;
        }

        uiCurrency.Clear();
        if (data != null)
        {
            var price = data.price;
            PlayerCurrency currencyData = null;
            switch (data.requirementType)
            {
                case InGamePackageRequirementType.RequireSoftCurrency:
                    currencyData = PlayerCurrency.SoftCurrency.Clone().SetAmount(price, 0);
                    break;
                case InGamePackageRequirementType.RequireHardCurrency:
                    currencyData = PlayerCurrency.HardCurrency.Clone().SetAmount(price, 0);
                    break;
            }
            uiCurrency.SetData(currencyData);
        }
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public void OnClickOpen()
    {
        var gameInstance = GameInstance.Singleton;
        var gameService = GameInstance.GameService;
        var price = data.price;
        switch (data.requirementType)
        {
            case InGamePackageRequirementType.RequireSoftCurrency:
                if (!PlayerCurrency.HaveEnoughSoftCurrency(price))
                {
                    gameInstance.WarnNotEnoughSoftCurrency();
                    return;
                }
                break;
            case InGamePackageRequirementType.RequireHardCurrency:
                if (!PlayerCurrency.HaveEnoughHardCurrency(price))
                {
                    gameInstance.WarnNotEnoughHardCurrency();
                    return;
                }
                break;
        }
        gameService.OpenInGamePackage(data.Id, OnOpenInGamePackageSuccess, OnOpenInGamePackageFail);
    }

    private void OnOpenInGamePackageSuccess(ItemResult result)
    {
        GameInstance.Singleton.OnGameServiceItemResult(result);
        if (result.rewardItems.Count > 0)
        {
            var lootBoxList = list as UILootBoxList;
            if (lootBoxList != null && lootBoxList.animItemsRewarding != null)
                lootBoxList.animItemsRewarding.Play(result.rewardItems);
            else
                GameInstance.Singleton.ShowRewardItemsDialog(result.rewardItems);
        }
    }

    private void OnOpenInGamePackageFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
    }
}
