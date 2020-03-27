using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClanManager : UIClan
{
    [Header("Manager settings")]
    public GameObject[] notJoinedClanObjects;
    public GameObject[] joinedClanObjects;
    public GameObject[] memberObjects;
    public GameObject[] managerObjects;
    public GameObject[] ownerObjects;
    public bool IsManager { get { return !IsEmpty() && Player.CurrentPlayer.ClanId.Equals(data.Id) && Player.CurrentPlayer.ClanRole == 1; } }
    public bool IsOwner { get { return !IsEmpty() && Player.CurrentPlayer.ClanId.Equals(data.Id) && Player.CurrentPlayer.ClanRole == 2; } }

    private void OnEnable()
    {
        RefreshData();
    }

    protected override void Update()
    {
        base.Update();
        foreach (var notJoinedClanObject in notJoinedClanObjects)
        {
            notJoinedClanObject.SetActive(IsEmpty());
        }
        foreach (var joinedClanObject in joinedClanObjects)
        {
            joinedClanObject.SetActive(!IsEmpty());
        }
        foreach (var memberObject in memberObjects)
        {
            memberObject.SetActive(!IsEmpty() && !IsManager && !IsOwner);
        }
        foreach (var managerObject in managerObjects)
        {
            managerObject.SetActive(!IsEmpty() && IsManager);
        }
        foreach (var ownerObject in ownerObjects)
        {
            ownerObject.SetActive(!IsEmpty() && IsOwner);
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
