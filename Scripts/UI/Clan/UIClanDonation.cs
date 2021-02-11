using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIClanDonation : UIDataItem<ClanDonation>
{
    public Image imageIcon;
    public UICurrency uiCurrency;
    public Text textRewardClanExp;
    public UIClanManager uiClanManager;
    [Header("Buttons")]
    public Button buttonDonate;
    [Header("Events")]
    public UnityEvent eventDonationSuccess;
    public UnityEvent eventDonationFail;

    public override void Clear()
    {
        if (imageIcon != null)
            imageIcon.sprite = null;

        if (uiCurrency != null)
            uiCurrency.SetData(new PlayerCurrency());

        if (textRewardClanExp != null)
            textRewardClanExp.text = "0";
    }

    public override bool IsEmpty()
    {
        return string.IsNullOrEmpty(data.id);
    }

    public override void UpdateData()
    {
        if (imageIcon != null)
            imageIcon.sprite = data.icon;

        if (uiCurrency != null)
        {
            uiCurrency.SetData(new PlayerCurrency()
            {
                DataId = data.requireCurrencyId,
                Amount = data.requireCurrencyAmount,
            });
        }

        if (textRewardClanExp != null)
            textRewardClanExp.text = data.rewardClanExp.ToString("N0");

        if (buttonDonate != null)
        {
            buttonDonate.onClick.RemoveListener(OnClickDonation);
            buttonDonate.onClick.AddListener(OnClickDonation);
            buttonDonate.interactable = !IsEmpty() && uiClanManager != null && !uiClanManager.Donated;
        }
    }

    public void OnClickDonation()
    {
        GameInstance.GameService.ClanDonation(data.id, OnDonationSuccess, OnDonationFail);
    }

    private void OnDonationSuccess(ClanDonationResult result)
    {
        if (eventDonationSuccess != null)
            eventDonationSuccess.Invoke();
        if (uiClanManager != null)
            uiClanManager.SetData(result.clan);
        PlayerCurrency.SetDataRange(result.updateCurrencies);
        if (uiClanManager != null)
            uiClanManager.RefreshDonationStatus();
    }

    private void OnDonationFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventDonationFail != null)
            eventDonationFail.Invoke();
    }
}
