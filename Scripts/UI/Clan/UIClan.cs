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
    [Header("Buttons")]
    public Button buttonJoinRequest;
    public Button buttonJoinRequestDelete;
    public Button buttonTerminate;
    public Button buttonExit;
    [Header("Events")]
    public UnityEvent eventJoinRequestSuccess;
    public UnityEvent eventJoinRequestFail;
    public UnityEvent eventJoinRequestDeleteSuccess;
    public UnityEvent eventJoinRequestDeleteFail;
    public UnityEvent eventTerminateSuccess;
    public UnityEvent eventTerminateFail;
    public UnityEvent eventExitSuccess;
    public UnityEvent eventExitFail;

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
            buttonJoinRequest.gameObject.SetActive(!IsEmpty());
        }
        if (buttonJoinRequestDelete != null)
        {
            buttonJoinRequestDelete.onClick.RemoveListener(OnClickJoinRequestDelete);
            buttonJoinRequestDelete.onClick.AddListener(OnClickJoinRequestDelete);
            buttonJoinRequestDelete.gameObject.SetActive(!IsEmpty());
        }
        if (buttonTerminate != null)
        {
            buttonTerminate.onClick.RemoveListener(OnClickTerminate);
            buttonTerminate.onClick.AddListener(OnClickTerminate);
            buttonTerminate.gameObject.SetActive(!IsEmpty());
        }
        if (buttonExit != null)
        {
            buttonExit.onClick.RemoveListener(OnClickExit);
            buttonExit.onClick.AddListener(OnClickExit);
            buttonExit.gameObject.SetActive(!IsEmpty());
        }
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
            uiLevel.collectExp = data.CollectExp;
            uiLevel.nextExp = data.NextExp;
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
}
