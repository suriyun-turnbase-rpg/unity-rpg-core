using System.Collections.Generic;
using UnityEngine.Events;

public class UIGenericUnlockableList : UIDataItemList<UIGenericUnlockable, GenericUnlockable>
{
    public void SetListItems(List<GenericUnlockable> list, UnityAction<UIGenericUnlockable> onSetListItem = null)
    {
        ClearListItems();
        foreach (var entry in list)
        {
            var ui = SetListItem(entry);
            if (ui != null && onSetListItem != null)
                onSetListItem(ui);
        }
    }

    public UIGenericUnlockable SetListItem(GenericUnlockable data)
    {
        if (data == null || string.IsNullOrEmpty(data.Id))
            return null;
        var item = SetListItem(data.Id);
        item.SetData(data);
        return item;
    }

    public bool ContainsItemWithDataId(string dataId)
    {
        MakeSelectedLists();
        var list = GetSelectedDataList();
        foreach (var entry in list)
        {
            if (entry.Id == dataId)
                return true;
        }
        return false;
    }
}
