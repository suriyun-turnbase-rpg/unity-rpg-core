using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemCraft : UIDataItem<ItemCraftFormula>
{
    public Text textTitle;
    public Text textDescription;
    public UIItem uiResultItem;
    public Text textAmount;
    public UICurrency uiPrice;
    public UIItemList uiMaterials;
    public Button buttonCraft;
    public UIItemCraftManager uiItemCraftManager;

    public override void Clear()
    {
        SetupInfo(null);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(ItemCraftFormula data)
    {
        if (textTitle != null)
            textTitle.text = data == null ? "" : data.title;

        if (textDescription != null)
            textDescription.text = data == null ? "" : data.description;

        if (uiResultItem != null)
        {
            uiResultItem.data = data == null ? null : new PlayerItem()
            {
                DataId = data.resultItem.Id,
                Amount = data.resultItem.amount,
            };
        }

        if (textAmount != null)
            textAmount.text = data == null ? "0" : data.resultItem.amount.ToString("N0");

        if (uiPrice != null)
        {
            uiPrice.Clear();
            PlayerCurrency currencyData = null;
            switch (data.requirementType)
            {
                case CraftRequirementType.RequireSoftCurrency:
                    currencyData = PlayerCurrency.SoftCurrency.Clone().SetAmount(data.price, 0);
                    break;
                case CraftRequirementType.RequireHardCurrency:
                    currencyData = PlayerCurrency.HardCurrency.Clone().SetAmount(data.price, 0);
                    break;
            }
            uiPrice.SetData(currencyData);
        }

        if (uiMaterials != null)
        {
            uiMaterials.ClearListItems();
            if (data != null && data.materials != null && data.materials.Length > 0)
            {
                var items = new List<PlayerItem>();
                for (var i = 0; i < data.materials.Length; ++i)
                {
                    var material = data.materials[i];
                    items.Add(new PlayerItem()
                    {
                        Id = i.ToString("N0"),
                        DataId = material.Id,
                        Amount = 0,
                    });
                }
                uiMaterials.SetListItems(items);
                for (var i = 0; i < data.materials.Length; ++i)
                {
                    var material = data.materials[i];
                    var ui = uiMaterials.GetListItem(i.ToString("N0"));
                    ui.SelectedAmount = PlayerItem.CountItem(Player.CurrentPlayerId, material.Id);
                    ui.RequiredAmount = material.amount;
                }
            }
        }
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public void OnClickCraft()
    {
        GameInstance.GameService.CraftItem(data.Id, OnClickCraftSuccess, OnClickCraftFail);
    }

    private void OnClickCraftSuccess(ItemResult result)
    {
        GameInstance.Singleton.OnGameServiceItemResult(result);
        uiItemCraftManager.ReloadList();
    }

    private void OnClickCraftFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
    }
}
