using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDailyReward : UIDataItem<RewardData>
{
    public UICurrency uiRewardCurrency;
    public UIItem uiRewardItem;
    public Text textDay;
    public GameObject isClaimedSignal;
    public GameObject canClaimSignal;
    public UIDailyRewardManager uiDailyRewardManager;
    public string Id { get; protected set; }
    public int Day { get; protected set; }
    public bool IsClaimed { get; protected set; }
    public bool CanClaim { get; protected set; }

    public override void Clear()
    {
        // Don't clear
    }

    public override bool IsEmpty()
    {
        // Never empty
        return false;
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(RewardData data)
    {
        bool shown = false;
        if (uiRewardCurrency != null)
        {
            uiRewardCurrency.Hide();
            if (data.currencies != null && data.currencies.Length > 0)
            {
                if (!shown)
                {
                    uiRewardCurrency.data = new PlayerCurrency()
                    {
                        DataId = data.currencies[0].id,
                        Amount = data.currencies[0].amount,
                    };
                    uiRewardCurrency.Show();
                    shown = true;
                }
            }
        }

        if (uiRewardItem != null)
        {
            uiRewardItem.Hide();
            if (data.items != null && data.items.Length > 0)
            {
                if (!shown)
                {
                    uiRewardItem.data = new PlayerItem()
                    {
                        DataId = data.items[0].item.Id,
                        Amount = data.items[0].amount,
                    };
                    uiRewardItem.Show();
                    shown = true;
                }
            }
        }
    }

    public void SetupClaimData(string id, int day, bool isClaimed, bool canClaim)
    {
        Id = id;
        Day = day;
        IsClaimed = isClaimed;
        CanClaim = canClaim;

        if (textDay != null)
            textDay.text = day.ToString("N0");

        if (isClaimedSignal != null)
            isClaimedSignal.SetActive(isClaimed);

        if (canClaimSignal != null)
            canClaimSignal.SetActive(canClaim);
    }

    public void OnClickClaim()
    {
        if (IsClaimed || !CanClaim)
            return;
        uiDailyRewardManager.OnClickClaim();
    }
}
