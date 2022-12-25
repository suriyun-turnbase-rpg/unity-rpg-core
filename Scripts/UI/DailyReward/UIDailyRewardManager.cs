using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDailyRewardManager : UIBase
{
    public DailyReward dailyReward;
    public UIDailyRewardList uiDailyRewardList;
    public GameObject canClaimSignal;
    public GameObject cannotClaimSignal;

    public override void Show()
    {
        base.Show();
        ReloadList();
    }

    public void ReloadList()
    {
        GameInstance.GameService.GetDailyRewardList(dailyReward.id, (result) =>
        {
            ReloadList(result.rewards);
        });
    }

    public void ReloadList(List<ClaimableDailyReward> rewards)
    {
        bool canClaim = false;
        bool[] isClaimedArray = new bool[rewards.Count];
        bool[] canClaimArray = new bool[rewards.Count];
        List<RewardData> list = new List<RewardData>();
        for (int i = 0; i < rewards.Count; ++i)
        {
            isClaimedArray[i] = rewards[i].IsClaimed;
            canClaimArray[i] = rewards[i].CanClaim;
            if (dailyReward != null && i < dailyReward.rewards.Length)
                list.Add(dailyReward.rewards[i]);
            if (canClaimArray[i])
                canClaim = true;
        }

        int index = 0;
        uiDailyRewardList.SetListItems(list, (ui) =>
        {
            ui.SetupClaimData(dailyReward.id, index + 1, isClaimedArray[index], canClaimArray[index]);
            ui.uiDailyRewardManager = this;
            index++;
        });

        if (canClaimSignal != null)
            canClaimSignal.SetActive(canClaim);

        if (cannotClaimSignal != null)
            cannotClaimSignal.SetActive(!canClaim);
    }

    public override void Hide()
    {
        base.Hide();
        if (uiDailyRewardList != null)
            uiDailyRewardList.ClearListItems();
    }

    public void OnClickClaim()
    {
        GameInstance.GameService.ClaimDailyReward(dailyReward.id, OnClickClaimSuccess, OnClickClaimFail);
    }

    private void OnClickClaimSuccess(GameServiceResult result)
    {
        ReloadList();
    }

    private void OnClickClaimFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
    }
}
