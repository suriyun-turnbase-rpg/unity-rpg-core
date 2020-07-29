using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIChatManager : UIBase
{
    public float reloadDuration = 1f;
    public bool isClanChat;
    public UIChatList uiChatList;
    public InputField messageInput;
    public KeyCode enterKey = KeyCode.Return;
    private float reloadCountDown = 0f;
    private long lastTime;

    public string Message { get => messageInput.text; set => messageInput.text = value; }

    private void Start()
    {
        lastTime = GameInstance.GameService.Timestamp;
    }

    public override void Show()
    {
        base.Show();
        Reload();
    }

    public override void Hide()
    {
        base.Hide();
        if (uiChatList != null)
            uiChatList.ClearListItems();
    }

    public void Reload()
    {
        GameInstance.GameService.GetChatMessages(lastTime, (result) =>
        {
            if (uiChatList != null)
            {
                uiChatList.selectable = false;
                uiChatList.multipleSelection = false;
                uiChatList.ClearListItems();
                uiChatList.SetListItems(result.list, (ui) =>
                {
                    ui.uiChatManager = this;
                });
            }
            lastTime = result.list.Last().createdAt;
        });
    }

    private void Update()
    {
        if (Input.GetKeyUp(enterKey))
        {
            OnClickSend();
        }
        reloadCountDown -= Time.deltaTime;
        if (reloadCountDown <= 0)
        {
            reloadCountDown = reloadDuration;
            Reload();
        }
    }

    public void OnClickSend()
    {
        if (string.IsNullOrEmpty(Message))
            return;
        messageInput.interactable = false;
        GameInstance.GameService.EnterChatMessage(isClanChat, Message, (result) =>
        {
            Message = string.Empty;
            messageInput.interactable = true;
        }, (error) =>
        {
            Message = string.Empty;
            messageInput.interactable = true;
            Debug.LogError("[EnterChatMessage] " + error);
        });
    }
}
