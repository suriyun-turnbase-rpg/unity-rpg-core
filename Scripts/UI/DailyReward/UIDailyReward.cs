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
    public bool CanClaimed { get; protected set; }

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
            if (data.currencies != null && data.currencies.Length > 0)
            {
                uiRewardCurrency.data = new PlayerCurrency()
                {
                    DataId = data.currencies[0].id,
                    Amount = data.currencies[0].amount,
                };
                if (!shown)
                {
                    uiRewardCurrency.Show();
                    shown = true;
                }
                else
                {
                    uiRewardCurrency.Hide();
                }
            }
        }

        if (uiRewardItem != null)
        {
            if (data.items != null && data.items.Length > 0)
            {
                uiRewardItem.data = new PlayerItem()
                {
                    DataId = data.items[0].item.id,
                    Amount = data.items[0].amount,
                };
                if (!shown)
                {
                    uiRewardItem.Show();
                    shown = true;
                }
                else
                {
                    uiRewardItem.Hide();
                }
            }
        }
    }

    public void SetupClaimData(string id, int day, bool isClaimed, bool canClaim)
    {
        Id = id;
        Day = day;
        IsClaimed = isClaimed;
        CanClaimed = canClaim;

        if (textDay != null)
            textDay.text = day.ToString("N0");

        if (isClaimedSignal != null)
            isClaimedSignal.SetActive(isClaimed);

        if (canClaimSignal != null)
            canClaimSignal.SetActive(canClaim);
    }

    public void OnClickClaim()
    {
        uiDailyRewardManager.OnClickClaim();
    }
}
