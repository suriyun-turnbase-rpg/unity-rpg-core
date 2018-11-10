using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIAPPackage : UIDataItem<IAPPackage>
{
    public Text textTitle;
    public Text textDescription;
    public Image imageIcon;
    public Text textPrice;

    public override void Clear()
    {
        SetupInfo(null);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(IAPPackage data)
    {
        if (textTitle != null)
            textTitle.text = data == null ? "" : data.title;

        if (textDescription != null)
            textDescription.text = data == null ? "" : data.description;

        if (imageIcon != null)
            imageIcon.sprite = data == null ? null : data.icon;
        
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public void OnClickOpen()
    {
        var gameInstance = GameInstance.Singleton;
        var gameService = GameInstance.GameService;
        if (!gameInstance.gameDatabase.IAPPackages.ContainsKey(data.Id))
            return;

        // TODO: add receipt / signature
        if (Application.platform == RuntimePlatform.Android)
            gameService.OpenIAPPackage_Android(data.Id, "", "", OnOpenIAPPackageSuccess, OnOpenIAPPackageFail);
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            gameService.OpenIAPPackage_iOS(data.Id, "", OnOpenIAPPackageSuccess, OnOpenIAPPackageFail);
    }

    private void OnOpenIAPPackageSuccess(ItemResult result)
    {
        GameInstance.Singleton.OnGameServiceItemResult(result);
        var updateCurrencies = result.updateCurrencies;
        foreach (var updateCurrency in updateCurrencies)
        {
            PlayerCurrency.SetData(updateCurrency);
        }
        var items = new List<PlayerItem>();
        items.AddRange(result.createItems);
        items.AddRange(result.updateItems);
        if (items.Count > 0)
            GameInstance.Singleton.ShowRewardItemsDialog(items);
    }

    private void OnOpenIAPPackageFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
    }
}
