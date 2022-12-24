using System.Collections.Generic;

public class UIUnlockTitleManager : UIBase
{
    public UIGenericUnlockable uiSelectedInfo;
    public UIGenericUnlockableList uiGenericUnlockableList;

    public override void Show()
    {
        base.Show();
        ReloadList();
    }

    public void ReloadList()
    {
        uiGenericUnlockableList.selectionMode = UIDataItemSelectionMode.Toggle;
        uiGenericUnlockableList.eventSelect.RemoveListener(SelectItem);
        uiGenericUnlockableList.eventSelect.AddListener(SelectItem);
        uiGenericUnlockableList.eventDeselect.RemoveListener(DeselectItem);
        uiGenericUnlockableList.eventDeselect.AddListener(DeselectItem);
        uiGenericUnlockableList.ClearListItems();
        int i = 0;
        uiGenericUnlockableList.SetListItems(new List<GenericUnlockable>(GameInstance.GameDatabase.PlayerTitles.Values), (ui) =>
        {
            ui.unlockDetectFunction = PlayerUnlockTitle.IsUnlock;
            if (i == 0 || Player.CurrentPlayer.TitleId == ui.data.Id)
                ui.OnClick();
            i++;
        });
        GameInstance.GameService.GetUnlockTitleList();
    }

    public override void Hide()
    {
        base.Hide();
        if (uiGenericUnlockableList != null)
            uiGenericUnlockableList.ClearListItems();
    }

    protected virtual void SelectItem(UIDataItem ui)
    {
        if (uiSelectedInfo != null)
            uiSelectedInfo.SetData((ui as UIGenericUnlockable).data);
    }

    protected virtual void DeselectItem(UIDataItem ui)
    {
        // Don't deselect
        ui.Selected = true;
    }

    public void OnClickUse()
    {
        string id = uiGenericUnlockableList.GetSelectedUIList()[0].data.Id;
        GameInstance.GameService.SetPlayerTitle(id, (result) =>
        {
            Player.CurrentPlayer.TitleId = id;
        });
    }
}
