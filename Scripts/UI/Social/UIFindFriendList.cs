using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFindFriendList : UIPlayerList
{
    public InputField inputDisplayName;

    private void OnEnable()
    {
        inputDisplayName.text = "";
        ClearListItems();
    }

    public void OnClickFind()
    {
        GameInstance.GameService.FindUser(inputDisplayName.text, OnFindUserSuccess, OnFindUserFail);
    }

    private void OnFindUserSuccess(PlayerListResult result)
    {
        SetListItems(result.list);
    }

    private void OnFindUserFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error);
    }
}
