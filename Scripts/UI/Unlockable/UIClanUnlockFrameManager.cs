using System.Collections.Generic;

public class UIClanUnlockFrameManager : UIBase
{
    public UIGenericUnlockable uiSelectedInfo;
    public UIGenericUnlockableList uiGenericUnlockableList;
    public UIClanManager uiClanManager;

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
        uiGenericUnlockableList.SetListItems(new List<GenericUnlockable>(GameInstance.GameDatabase.ClanFrames.Values), (ui) =>
        {
            ui.unlockDetectFunction = ClanUnlockFrame.IsUnlock;
            if (i == 0 || uiClanManager.data.FrameId == ui.data.Id)
                ui.OnClick();
            i++;
        });
        GameInstance.GameService.GetClanUnlockFrameList();
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
        GameInstance.GameService.SetClanFrame(id, (result) =>
        {
            uiClanManager.data.FrameId = id;
        });
    }
}
