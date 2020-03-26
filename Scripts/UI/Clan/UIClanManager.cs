using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClanManager : UIClan
{
    public bool IsOwner { get { return !IsEmpty() && Player.CurrentPlayerId.Equals(data.OwnerId); } }
    public UIClanMemberList uiClanMemberList;
    public UIClanJoinRequestList uiClanJoinRequestList;
    public UIClanJoinPendingRequestList uiClanJoinPendingRequestList;

    public GameObject[] joinedObjects;
    public GameObject[] notJoinedObjects;

    private void OnEnable()
    {
        RefreshData();
    }

    protected override void Update()
    {
        base.Update();
        foreach (var joinedObject in joinedObjects)
        {
            joinedObject.SetActive(!IsEmpty());
        }
        foreach (var notJoinedObject in notJoinedObjects)
        {
            notJoinedObject.SetActive(IsEmpty());
        }
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
