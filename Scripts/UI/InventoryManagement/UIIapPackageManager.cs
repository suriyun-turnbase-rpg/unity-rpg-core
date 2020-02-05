using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIIapPackageManager : UIBase
{
    public UIIapPackageList uiIapPackageList;
    public List<string> categories;

    public override void Show()
    {
        base.Show();

        if (uiIapPackageList != null)
        {
            var availableIAPPackagees = GameInstance.AvailableIapPackages;
            var allIAPPackagees = GameInstance.GameDatabase.IapPackages;
            var list = allIAPPackagees.Values.Where(a => availableIAPPackagees.Contains(a.Id) && ContainCategory(a.category)).ToList();
            uiIapPackageList.SetListItems(list);
        }
    }

    public bool ContainCategory(string category)
    {
        if (categories == null || categories.Count == 0)
            return true;
        return categories.Contains(category);
    }
}
