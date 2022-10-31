using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class UIDailyRewardList : UIDataItemList<UIDailyReward, RewardData>
{
    public void SetListItems(List<RewardData> list, UnityAction<UIDailyReward> onSetListItem = null)
    {
        ClearListItems();
        foreach (var entry in list)
        {
            var ui = SetListItem(entry);
            if (ui != null && onSetListItem != null)
                onSetListItem(ui);
        }
    }

    public UIDailyReward SetListItem(RewardData data)
    {
        string id = System.Guid.NewGuid().ToString();
        var item = SetListItem(id);
        item.SetData(data);
        return item;
    }
}
