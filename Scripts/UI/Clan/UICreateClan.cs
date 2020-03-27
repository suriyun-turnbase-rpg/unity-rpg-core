using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UICreateClan : UIBase
{
    public InputField inputClanName;
    public UICurrency uiRequireCurrency;
    public UIClanManager manager;

    private void OnEnable()
    {
        inputClanName.text = "";
    }

    public void OnClickCreate()
    {
        GameInstance.GameService.CreateClan(inputClanName.text, OnCreateClanSuccess, OnCreateClanFail);
    }

    private void OnCreateClanSuccess(CreateClanResult result)
    {
        manager.RefreshData();
    }

    private void OnCreateClanFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
    }
}
