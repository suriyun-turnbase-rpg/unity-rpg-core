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
    
    private byte? previousClanRole;

    private void OnEnable()
    {
        RefreshData();
    }

    protected override void Update()
    {
        base.Update();
        if (!previousClanRole.HasValue || previousClanRole.Value != Player.CurrentPlayer.ClanRole)
        {
            previousClanRole = Player.CurrentPlayer.ClanRole;
            UpdateState();
        }
    }

    private void UpdateState()
    {
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
            memberObject.SetActive(false);
        }
        foreach (var managerObject in managerObjects)
        {
            managerObject.SetActive(false);
        }
        foreach (var ownerObject in ownerObjects)
        {
            ownerObject.SetActive(false);
        }
        switch (Player.CurrentPlayer.ClanRole)
        {
            case 0:
                foreach (var memberObject in memberObjects)
                {
                    memberObject.SetActive(true);
                }
                break;
            case 1:
                foreach (var managerObject in managerObjects)
                {
                    managerObject.SetActive(true);
                }
                break;
            case 2:
                foreach (var ownerObject in ownerObjects)
                {
                    ownerObject.SetActive(true);
                }
                break;
        }
    }

    public override void UpdateData()
    {
        base.UpdateData();
        UpdateState();
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
