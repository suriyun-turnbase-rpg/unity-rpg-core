using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIIAPPackageList : UIDataItemList<UIIAPPackage, IAPPackage>
{
    public void SetListItems(List<IAPPackage> list, UnityAction<UIIAPPackage> onSetListItem = null)
    {
        ClearListItems();
        foreach (var entry in list)
        {
            var ui = SetListItem(entry);
            if (ui != null && onSetListItem != null)
                onSetListItem(ui);
        }
    }

    public UIIAPPackage SetListItem(IAPPackage data)
    {
        if (data == null || string.IsNullOrEmpty(data.Id))
            return null;
        var item = SetListItem(data.Id);
        item.SetData(data);
        return item;
    }
}
