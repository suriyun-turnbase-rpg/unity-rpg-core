using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class UIPlayer : UIDataItem<Player>
{
    public Text textProfileName;
    public UILevel uiLevel;
    public UIItem uiMainCharacter;
    public UIPlayerList uiPlayerList;
    [Header("Friend buttons")]
    public Button buttonFriendRequest;
    public Button buttonFriendAccept;
    public Button buttonFriendDecline;
    public Button buttonFriendDelete;
    [FormerlySerializedAs("buttonRequestDelete")]
    public Button buttonFriendRequestDelete;
    [Header("Clan buttons")]
    public Button buttonClanJoinAccept;
    public Button buttonClanJoinDecline;
    public Button buttonClanMemberDelete;
    public Button buttonClanOwnerTransfer;
    public Button buttonClanJoinRequestDelete;
    [Header("Arena UIs")]
    public Text textArenaScore;
    public UIArenaRank uiArenaRank;
    // Events
    [Header("Friend events")]
    public UnityEvent eventFriendRequestSuccess;
    public UnityEvent eventFriendRequestFail;
    public UnityEvent eventFriendAcceptSuccess;
    public UnityEvent eventFriendAcceptFail;
    public UnityEvent eventFriendDeclineSuccess;
    public UnityEvent eventFriendDeclineFail;
    public UnityEvent eventFriendDeleteSuccess;
    public UnityEvent eventFriendDeleteFail;
    public UnityEvent eventFriendRequestDeleteSuccess;
    public UnityEvent eventFriendRequestDeleteFail;
    [Header("Clan events")]
    public UnityEvent eventClanJoinAcceptSuccess;
    public UnityEvent eventClanJoinAcceptFail;
    public UnityEvent eventClanJoinDeclineSuccess;
    public UnityEvent eventClanJoinDeclineFail;
    public UnityEvent eventClanMemberDeleteSuccess;
    public UnityEvent eventClanMemberDeleteFail;
    public UnityEvent eventClanOwnerTransferSuccess;
    public UnityEvent eventClanOwnerTransferFail;
    public UnityEvent eventClanJoinRequestDeleteSuccess;
    public UnityEvent eventClanJoinRequestDeleteFail;

    public override void UpdateData()
    {
        SetupInfo(data);
        // Friend buttons
        if (buttonFriendRequest != null)
        {
            buttonFriendRequest.onClick.RemoveListener(OnClickFriendRequest);
            buttonFriendRequest.onClick.AddListener(OnClickFriendRequest);
            buttonFriendRequest.gameObject.SetActive(!IsEmpty());
        }
        if (buttonFriendAccept != null)
        {
            buttonFriendAccept.onClick.RemoveListener(OnClickFriendAccept);
            buttonFriendAccept.onClick.AddListener(OnClickFriendAccept);
            buttonFriendAccept.gameObject.SetActive(!IsEmpty());
        }
        if (buttonFriendDecline != null)
        {
            buttonFriendDecline.onClick.RemoveListener(OnClickFriendDecline);
            buttonFriendDecline.onClick.AddListener(OnClickFriendDecline);
            buttonFriendDecline.gameObject.SetActive(!IsEmpty());
        }
        if (buttonFriendDelete != null)
        {
            buttonFriendDelete.onClick.RemoveListener(OnClickFriendDelete);
            buttonFriendDelete.onClick.AddListener(OnClickFriendDelete);
            buttonFriendDelete.gameObject.SetActive(!IsEmpty());
        }
        if (buttonFriendRequestDelete != null)
        {
            buttonFriendRequestDelete.onClick.RemoveListener(OnClickRequestDelete);
            buttonFriendRequestDelete.onClick.AddListener(OnClickRequestDelete);
            buttonFriendRequestDelete.gameObject.SetActive(!IsEmpty());
        }
        // Clan buttons
        if (buttonClanJoinAccept != null)
        {
            buttonClanJoinAccept.onClick.RemoveListener(OnClickClanJoinAccept);
            buttonClanJoinAccept.onClick.AddListener(OnClickClanJoinAccept);
            buttonClanJoinAccept.gameObject.SetActive(!IsEmpty());
        }
        if (buttonClanJoinDecline != null)
        {
            buttonClanJoinDecline.onClick.RemoveListener(OnClickClanJoinDecline);
            buttonClanJoinDecline.onClick.AddListener(OnClickClanJoinDecline);
            buttonClanJoinDecline.gameObject.SetActive(!IsEmpty());
        }
        if (buttonClanMemberDelete != null)
        {
            buttonClanMemberDelete.onClick.RemoveListener(OnClickClanMemberDelete);
            buttonClanMemberDelete.onClick.AddListener(OnClickClanMemberDelete);
            buttonClanMemberDelete.gameObject.SetActive(!IsEmpty());
        }
        if (buttonClanOwnerTransfer != null)
        {
            buttonClanOwnerTransfer.onClick.RemoveListener(OnClickClanOwnerTransfer);
            buttonClanOwnerTransfer.onClick.AddListener(OnClickClanOwnerTransfer);
            buttonClanOwnerTransfer.gameObject.SetActive(!IsEmpty());
        }
        if (buttonClanJoinRequestDelete != null)
        {
            buttonClanJoinRequestDelete.onClick.RemoveListener(OnClickClanJoinRequestDelete);
            buttonClanJoinRequestDelete.onClick.AddListener(OnClickClanJoinRequestDelete);
            buttonClanJoinRequestDelete.gameObject.SetActive(!IsEmpty());
        }
    }

    public override void Clear()
    {
        SetupInfo(null);
    }

    private void SetupInfo(Player data)
    {
        if (data == null)
            data = new Player();

        if (textProfileName != null)
            textProfileName.text = data.ProfileName;

        // Stats
        if (uiLevel != null)
        {
            uiLevel.level = data.Level;
            uiLevel.maxLevel = data.MaxLevel;
            uiLevel.collectExp = data.CollectExp;
            uiLevel.nextExp = data.NextExp;
        }

        if (uiMainCharacter != null)
        {
            if (string.IsNullOrEmpty(data.MainCharacter) || !GameInstance.GameDatabase.Items.ContainsKey(data.MainCharacter))
            {
                uiMainCharacter.data = null;
            }
            else
            {
                uiMainCharacter.data = new PlayerItem()
                {
                    DataId = data.MainCharacter,
                    Exp = data.MainCharacterExp,
                };
            }
        }

        if (textArenaScore != null)
            textArenaScore.text = data.ArenaScore.ToString("N0");

        if (uiArenaRank != null)
            uiArenaRank.SetData(data.ArenaRank);
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public void OnClickFriendRequest()
    {
        GameInstance.GameService.FriendRequest(data.Id, OnFriendRequestSuccess, OnFriendRequestFail);
    }

    private void OnFriendRequestSuccess(GameServiceResult result)
    {
        if (eventFriendRequestSuccess != null)
            eventFriendRequestSuccess.Invoke();
    }

    private void OnFriendRequestFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventFriendRequestFail != null)
            eventFriendRequestFail.Invoke();
    }

    public void OnClickFriendAccept()
    {
        GameInstance.GameService.FriendAccept(data.Id, OnFriendAcceptSuccess, OnFriendAcceptFail);
    }

    private void OnFriendAcceptSuccess(GameServiceResult result)
    {
        if (uiPlayerList != null)
            uiPlayerList.RemoveListItem(data.Id);
        if (eventFriendAcceptSuccess != null)
            eventFriendAcceptSuccess.Invoke();
    }

    private void OnFriendAcceptFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventFriendAcceptFail != null)
            eventFriendAcceptFail.Invoke();
    }

    public void OnClickFriendDecline()
    {
        GameInstance.GameService.FriendDecline(data.Id, OnFriendDeclineSuccess, OnFriendDeclineFail);
    }

    private void OnFriendDeclineSuccess(GameServiceResult result)
    {
        if (uiPlayerList != null)
            uiPlayerList.RemoveListItem(data.Id);
        if (eventFriendDeclineSuccess != null)
            eventFriendDeclineSuccess.Invoke();
    }

    private void OnFriendDeclineFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventFriendDeclineFail != null)
            eventFriendDeclineFail.Invoke();
    }

    public void OnClickFriendDelete()
    {
        GameInstance.GameService.FriendDelete(data.Id, OnFriendDeleteSuccess, OnFriendDeleteFail);
    }

    private void OnFriendDeleteSuccess(GameServiceResult result)
    {
        if (uiPlayerList != null)
            uiPlayerList.RemoveListItem(data.Id);
        if (eventFriendDeleteSuccess != null)
            eventFriendDeleteSuccess.Invoke();
    }

    private void OnFriendDeleteFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventFriendDeleteFail != null)
            eventFriendDeleteFail.Invoke();
    }

    public void OnClickRequestDelete()
    {
        GameInstance.GameService.FriendRequestDelete(data.Id, OnRequestDeleteSuccess, OnRequestDeleteFail);
    }

    private void OnRequestDeleteSuccess(GameServiceResult result)
    {
        if (uiPlayerList != null)
            uiPlayerList.RemoveListItem(data.Id);
        if (eventFriendRequestDeleteSuccess != null)
            eventFriendRequestDeleteSuccess.Invoke();
    }

    private void OnRequestDeleteFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventFriendRequestDeleteFail != null)
            eventFriendRequestDeleteFail.Invoke();
    }

    public void OnClickClanJoinAccept()
    {
        GameInstance.GameService.ClanJoinAccept(data.Id, ClanJoinAcceptSuccess, ClanJoinAcceptFail);
    }

    private void ClanJoinAcceptSuccess(GameServiceResult result)
    {
        if (uiPlayerList != null)
            uiPlayerList.RemoveListItem(data.Id);
        if (eventClanJoinAcceptSuccess != null)
            eventClanJoinAcceptSuccess.Invoke();
    }

    private void ClanJoinAcceptFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventClanJoinAcceptFail != null)
            eventClanJoinAcceptFail.Invoke();
    }
    public void OnClickClanJoinDecline()
    {
        GameInstance.GameService.ClanJoinDecline(data.Id, ClanJoinDeclineSuccess, ClanJoinDeclineFail);
    }

    private void ClanJoinDeclineSuccess(GameServiceResult result)
    {
        if (uiPlayerList != null)
            uiPlayerList.RemoveListItem(data.Id);
        if (eventClanJoinDeclineSuccess != null)
            eventClanJoinDeclineSuccess.Invoke();
    }

    private void ClanJoinDeclineFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventClanJoinDeclineFail != null)
            eventClanJoinDeclineFail.Invoke();
    }

    public void OnClickClanMemberDelete()
    {
        GameInstance.GameService.ClanMemberDelete(data.Id, ClanMemberDeleteSuccess, ClanMemberDeleteFail);
    }

    private void ClanMemberDeleteSuccess(GameServiceResult result)
    {
        if (uiPlayerList != null)
            uiPlayerList.RemoveListItem(data.Id);
        if (eventClanMemberDeleteSuccess != null)
            eventClanMemberDeleteSuccess.Invoke();
    }

    private void ClanMemberDeleteFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventClanMemberDeleteFail != null)
            eventClanMemberDeleteFail.Invoke();
    }

    public void OnClickClanOwnerTransfer()
    {
        GameInstance.GameService.ClanOwnerTransfer(data.Id, ClanOwnerTransferSuccess, ClanOwnerTransferFail);
    }

    private void ClanOwnerTransferSuccess(GameServiceResult result)
    {
        if (eventClanOwnerTransferSuccess != null)
            eventClanOwnerTransferSuccess.Invoke();
    }

    private void ClanOwnerTransferFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventClanOwnerTransferFail != null)
            eventClanOwnerTransferFail.Invoke();
    }

    public void OnClickClanJoinRequestDelete()
    {
        GameInstance.GameService.ClanJoinRequestDelete(data.Id, ClanJoinRequestDeleteSuccess, ClanJoinRequestDeleteFail);
    }

    private void ClanJoinRequestDeleteSuccess(GameServiceResult result)
    {
        if (uiPlayerList != null)
            uiPlayerList.RemoveListItem(data.Id);
        if (eventClanJoinRequestDeleteSuccess != null)
            eventClanJoinRequestDeleteSuccess.Invoke();
    }

    private void ClanJoinRequestDeleteFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
        if (eventClanJoinRequestDeleteFail != null)
            eventClanJoinRequestDeleteFail.Invoke();
    }
}
