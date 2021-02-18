using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClanManager : UIClan
{
    [Header("Manager settings")]
    public UIClanDonationList uiUIClanDonationList;
    public GameObject[] notJoinedClanObjects;
    public GameObject[] joinedClanObjects;
    public GameObject[] memberObjects;
    public GameObject[] managerObjects;
    public GameObject[] ownerObjects;
    public GameObject[] notCheckedInObjects;
    public GameObject[] checkedInObjects;
    public GameObject[] notDonatedObjects;
    public GameObject[] donatedObjects;
    public bool IsManager { get { return !IsEmpty() && Player.CurrentPlayer.ClanId.Equals(data.Id) && Player.CurrentPlayer.ClanRole == 1; } }
    public bool IsOwner { get { return !IsEmpty() && Player.CurrentPlayer.ClanId.Equals(data.Id) && Player.CurrentPlayer.ClanRole == 2; } }

    private byte? previousClanRole;

    private void OnEnable()
    {
        if (uiUIClanDonationList != null)
        {
            uiUIClanDonationList.uiClanManager = this;
            uiUIClanDonationList.SetListItems(GameInstance.GameDatabase.clanDonations);
        }
        RefreshData();
        RefreshCheckinStatus();
        RefreshDonationStatus();
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
        if (notJoinedClanObjects != null)
        {
            foreach (var notJoinedClanObject in notJoinedClanObjects)
            {
                notJoinedClanObject.SetActive(IsEmpty());
            }
        }
        if (joinedClanObjects != null)
        {
            foreach (var joinedClanObject in joinedClanObjects)
            {
                joinedClanObject.SetActive(!IsEmpty());
            }
        }
        if (memberObjects != null)
        {
            foreach (var memberObject in memberObjects)
            {
                memberObject.SetActive(false);
            }
        }
        if (managerObjects != null)
        {
            foreach (var managerObject in managerObjects)
            {
                managerObject.SetActive(false);
            }
        }
        if (ownerObjects != null)
        {
            foreach (var ownerObject in ownerObjects)
            {
                ownerObject.SetActive(false);
            }
        }
        switch (Player.CurrentPlayer.ClanRole)
        {
            case 0:
                if (memberObjects != null)
                {
                    foreach (var memberObject in memberObjects)
                    {
                        memberObject.SetActive(true);
                    }
                }
                break;
            case 1:
                if (managerObjects != null)
                {
                    foreach (var managerObject in managerObjects)
                    {
                        managerObject.SetActive(true);
                    }
                }
                break;
            case 2:
                if (ownerObjects != null)
                {
                    foreach (var ownerObject in ownerObjects)
                    {
                        ownerObject.SetActive(true);
                    }
                }
                break;
        }
        if (notCheckedInObjects != null)
        {
            foreach (var notCheckedInObject in notCheckedInObjects)
            {
                notCheckedInObject.SetActive(IsEmpty() || !CheckedIn);
            }
        }
        if (checkedInObjects != null)
        {
            foreach (var checkedInObject in checkedInObjects)
            {
                checkedInObject.SetActive(!IsEmpty() && CheckedIn);
            }
        }
        if (notDonatedObjects != null)
        {
            foreach (var notDonatedObject in notDonatedObjects)
            {
                notDonatedObject.SetActive(IsEmpty() || !DonatedReachedLimit);
            }
        }
        if (donatedObjects != null)
        {
            foreach (var donatedObject in donatedObjects)
            {
                donatedObject.SetActive(!IsEmpty() && DonatedReachedLimit);
            }
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

    public void RefreshCheckinStatus()
    {
        GameInstance.GameService.GetClanCheckinStatus(OnRefreshCheckinStatusSuccess);
    }

    private void OnRefreshCheckinStatusSuccess(ClanCheckinStatusResult result)
    {
        CheckedIn = result.alreadyCheckin;
        UpdateState();
        ForceUpdate();
    }

    public void RefreshDonationStatus()
    {
        GameInstance.GameService.GetClanDonationStatus(OnRefreshDonationStatusSuccess);
    }

    private void OnRefreshDonationStatusSuccess(ClanDonationStatusResult result)
    {
        ClanDonateCount = result.clanDonateCount;
        MaxClanDonation = result.maxClanDonation;
        UpdateState();
        ForceUpdate();
        if (uiUIClanDonationList != null)
        {
            uiUIClanDonationList.ClearListItems();
            uiUIClanDonationList.SetListItems(GameInstance.GameDatabase.clanDonations);
        }
    }
}
