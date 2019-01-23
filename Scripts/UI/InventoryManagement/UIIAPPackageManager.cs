using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIIapPackageManager : UIBase
{
    public UIIapPackageList uiIAPPackageList;

    public override void Show()
    {
        base.Show();

        if (uiIAPPackageList != null)
        {
            var availableIAPPackagees = GameInstance.AvailableIAPPackages;
            var allIAPPackagees = GameInstance.GameDatabase.IapPackages;
            var list = allIAPPackagees.Values.Where(a => availableIAPPackagees.Contains(a.Id)).ToList();
            uiIAPPackageList.SetListItems(list);
        }
    }
}
