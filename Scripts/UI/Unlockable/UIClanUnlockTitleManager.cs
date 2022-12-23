using System.Collections.Generic;

public class UIClanUnlockTitleManager : UIBase
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
        uiGenericUnlockableList.SetListItems(new List<GenericUnlockable>(GameInstance.GameDatabase.ClanTitles.Values), (ui) =>
        {
            ui.unlockDetectFunction = ClanUnlockTitle.IsUnlock;
        });
        GameInstance.GameService.GetClanUnlockTitleList();
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

    }
}
