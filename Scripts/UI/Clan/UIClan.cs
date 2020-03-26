using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIClan : UIDataItem<Clan>
{
    public Text textName;
    public UIPlayer uiOwner;
    public UIClanList uiClanList;
    public Button buttonJoinRequest;
    public Button buttonJoinRequestDelete;
    public UnityEvent eventJoinRequestSuccess;
    public UnityEvent eventJoinRequestFail;
    public UnityEvent eventJoinRequestDeleteSuccess;
    public UnityEvent eventJoinRequestDeleteFail;

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
    }

    private void SetupInfo(Clan data)
    {
        if (textName != null)
            textName.text = data.Name;

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
        GameInstance.GameService.ClanJoinRequestDelete(data.Id, OnJoinRequestDeleteSuccess, OnJoinRequestDeleteFail);
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
}
