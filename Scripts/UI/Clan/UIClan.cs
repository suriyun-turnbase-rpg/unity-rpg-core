using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIClan : UIDataItem<Clan>
{
    public Text textName;
    public UILevel uiLevel;
    public UIPlayer uiOwner;
    public UIClanList uiClanList;
    public Text textClanDonateCount;
    public Text textMaxClanDonation;
    [Header("Buttons")]
    public Button buttonJoinRequest;
    public Button buttonJoinRequestDelete;
    public Button buttonTerminate;
    public Button buttonExit;
    public Button buttonCheckin;
    [Header("Events")]
    public UnityEvent eventJoinRequestSuccess;
    public UnityEvent eventJoinRequestFail;
    public UnityEvent eventJoinRequestDeleteSuccess;
    public UnityEvent eventJoinRequestDeleteFail;
    public UnityEvent eventTerminateSuccess;
    public UnityEvent eventTerminateFail;
    public UnityEvent eventExitSuccess;
    public UnityEvent eventExitFail;
    public UnityEvent eventCheckinSuccess;
    public UnityEvent eventCheckinFail;

    public bool CheckedIn { get; protected set; }
    public int ClanDonateCount { get; protected set; }
    public int MaxClanDonation { get; protected set; }
    public bool DonatedReachedLimit { get { return ClanDonateCount >= MaxClanDonation; } }

    public override void Clear()
    {
        SetupInfo(null);
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
        if (buttonJoinRequest != null)
        {
            buttonJoinRequest.onClick.RemoveListener(OnClickJoinRequest);
            buttonJoinRequest.onClick.AddListener(OnClickJoinRequest);
            buttonJoinRequest.interactable = !IsEmpty() && !Player.CurrentPlayer.JoinedClan;
        }
        if (buttonJoinRequestDelete != null)
        {
            buttonJoinRequestDelete.onClick.RemoveListener(OnClickJoinRequestDelete);
            buttonJoinRequestDelete.onClick.AddListener(OnClickJoinRequestDelete);
            buttonJoinRequestDelete.interactable = !IsEmpty() && !Player.CurrentPlayer.JoinedClan;
        }
        if (buttonTerminate != null)
        {
            buttonTerminate.onClick.RemoveListener(OnClickTerminate);
            buttonTerminate.onClick.AddListener(OnClickTerminate);
            buttonTerminate.interactable = !IsEmpty() && Player.CurrentPlayer.IsClanLeader && Player.CurrentPlayer.ClanId.Equals(data.Id);
        }
        if (buttonExit != null)
        {
            buttonExit.onClick.RemoveListener(OnClickExit);
            buttonExit.onClick.AddListener(OnClickExit);
            buttonExit.interactable = !IsEmpty() && !Player.CurrentPlayer.IsClanLeader && Player.CurrentPlayer.ClanId.Equals(data.Id);
        }
        if (buttonCheckin != null)
        {
            buttonCheckin.onClick.RemoveListener(OnClickCheckin);
            buttonCheckin.onClick.AddListener(OnClickCheckin);
            buttonCheckin.interactable = !IsEmpty() && Player.CurrentPlayer.ClanId.Equals(data.Id) && !CheckedIn;
        }
        if (textClanDonateCount != null)
            textClanDonateCount.text = ClanDonateCount.ToString("N0");
        if (textMaxClanDonation != null)
            textMaxClanDonation.text = MaxClanDonation > 0 ? MaxClanDonation.ToString("N0") : GameInstance.GameDatabase.maxClanDonation.ToString("N0");
    }

    private void SetupInfo(Clan data)
    {
        if (data == null)
            data = new Clan();

        if (textName != null)
            textName.text = data.Name;

        if (uiLevel != null)
        {
            uiLevel.level = data.Level;
            uiLevel.maxLevel = data.MaxLevel;
            uiLevel.currentExp = data.CurrentExp;
            uiLevel.requiredExp = data.RequiredExp;
        }

        if (uiOwner != null)
            uiOwner.data = data.Owner;
    }

    public void OnClickJoinRequest()
    {
        GameInstance.GameService.ClanJoinRequest(data.Id, OnJoinRequestSuccess, OnJoinRequestFail);
    }

    private void OnJoinRequestSuccess(GameServiceResult result)
    {
        if (uiClanList != null)
            uiClanList.RemoveListItem(data.Id);
        if (eventJoinRequestSuccess != null)
            eventJoinRequestSuccess.Invoke();
    }

    private void OnJoinRequestFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventJoinRequestFail != null)
            eventJoinRequestFail.Invoke();
    }

    public void OnClickJoinRequestDelete()
    {
        GameInstance.Singleton.ShowConfirmDialog(
            LanguageManager.GetText(GameText.WARN_TITLE_DELETE_CLAN_JOIN_REQUEST),
            LanguageManager.GetText(GameText.WARN_DESCRIPTION_DELETE_CLAN_JOIN_REQUEST),
            () =>
            {
                GameInstance.GameService.ClanJoinRequestDelete(data.Id, OnJoinRequestDeleteSuccess, OnJoinRequestDeleteFail);
            });
    }

    private void OnJoinRequestDeleteSuccess(GameServiceResult result)
    {
        if (uiClanList != null)
            uiClanList.RemoveListItem(data.Id);
        if (eventJoinRequestDeleteSuccess != null)
            eventJoinRequestDeleteSuccess.Invoke();
    }

    private void OnJoinRequestDeleteFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventJoinRequestDeleteFail != null)
            eventJoinRequestDeleteFail.Invoke();
    }

    public void OnClickTerminate()
    {
        GameInstance.Singleton.ShowConfirmDialog(
            LanguageManager.GetText(GameText.WARN_TITLE_CLAN_TERMINATE),
            LanguageManager.GetText(GameText.WARN_DESCRIPTION_CLAN_TERMINATE),
            () =>
            {
                GameInstance.GameService.ClanTerminate(OnTerminateSuccess, OnTerminateFail);
            });
    }

    private void OnTerminateSuccess(GameServiceResult result)
    {
        if (eventTerminateSuccess != null)
            eventTerminateSuccess.Invoke();
    }

    private void OnTerminateFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventTerminateFail != null)
            eventTerminateFail.Invoke();
    }

    public void OnClickExit()
    {
        GameInstance.Singleton.ShowConfirmDialog(
            LanguageManager.GetText(GameText.WARN_TITLE_CLAN_EXIT),
            LanguageManager.GetText(GameText.WARN_DESCRIPTION_CLAN_EXIT),
            () =>
            {
                GameInstance.GameService.ClanExit(OnExitSuccess, OnExitFail);
            });
    }

    private void OnExitSuccess(GameServiceResult result)
    {
        if (eventExitSuccess != null)
            eventExitSuccess.Invoke();
    }

    private void OnExitFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventExitFail != null)
            eventExitFail.Invoke();
    }

    public void OnClickCheckin()
    {
        GameInstance.GameService.ClanCheckin(OnCheckinSuccess, OnCheckinFail);
    }

    private void OnCheckinSuccess(ClanCheckinResult result)
    {
        if (eventCheckinSuccess != null)
            eventCheckinSuccess.Invoke();
        CheckedIn = true;
        SetData(result.clan);
        PlayerCurrency.SetDataRange(result.updateCurrencies);
    }

    private void OnCheckinFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventCheckinFail != null)
            eventCheckinFail.Invoke();
    }
}
