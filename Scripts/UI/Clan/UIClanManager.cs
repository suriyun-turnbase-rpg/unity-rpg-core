using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClanManager : UIClan
{
    private void OnEnable()
    {
        RefreshData();
    }

    public void RefreshData()
    {
        GameInstance.GameService.GetClan(OnRefreshSuccess, OnRefreshFail);
    }

    private void OnRefreshSuccess(ClanResult result)
    {
        SetData(result.clan);
    }

    private void OnRefreshFail(string error)
    {
        SetData(null);
    }
}
