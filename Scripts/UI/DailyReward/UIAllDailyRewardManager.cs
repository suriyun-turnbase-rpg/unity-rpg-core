using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAllDailyRewardManager : MonoBehaviour
{
    public UIDailyRewardManager[] uiDailyRewardManagers = new UIDailyRewardManager[0];

    public readonly Dictionary<string, UIDailyRewardManager> UIDailyRewardManagers = new Dictionary<string, UIDailyRewardManager>();
    private void Awake()
    {
        foreach (var uiDailyRewardManager in uiDailyRewardManagers)
        {
            UIDailyRewardManagers[uiDailyRewardManager.dailyReward.Id] = uiDailyRewardManager;
        }
    }

    private void Start()
    {
        GameInstance.GameService.GetAllDailyRewardList((result) =>
        {
            foreach (var dailyReward in result.dailyRewards)
            {
                if (UIDailyRewardManagers.TryGetValue(dailyReward.id, out var manager))
                {
                    manager.ReloadList(dailyReward.rewards);
                    manager.Show();
                }
            }
        });
    }
}
