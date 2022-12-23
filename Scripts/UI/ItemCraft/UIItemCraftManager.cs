using System.Collections.Generic;

public class UIItemCraftManager : UIBase
{
    public UIItemCraft uiSelectedInfo;
    public UIItemCraftList uiItemCraftList;

    public override void Show()
    {
        base.Show();
        ReloadList();
    }

    public void ReloadList()
    {
        uiItemCraftList.selectionMode = UIDataItemSelectionMode.Toggle;
        uiItemCraftList.eventSelect.RemoveListener(SelectItem);
        uiItemCraftList.eventSelect.AddListener(SelectItem);
        uiItemCraftList.eventDeselect.RemoveListener(DeselectItem);
        uiItemCraftList.eventDeselect.AddListener(DeselectItem);
        uiItemCraftList.ClearListItems();
        uiItemCraftList.SetListItems(new List<ItemCraftFormula>(GameInstance.GameDatabase.ItemCrafts.Values), (ui) =>
        {
            ui.uiItemCraftManager = this;
        });
    }

    public override void Hide()
    {
        base.Hide();
        if (uiItemCraftList != null)
            uiItemCraftList.ClearListItems();
    }

    protected virtual void SelectItem(UIDataItem ui)
    {
        if (uiSelectedInfo != null)
            uiSelectedInfo.SetData((ui as UIItemCraft).data);
    }

    protected virtual void DeselectItem(UIDataItem ui)
    {
        // Don't deselect
        ui.Selected = true;
    }
}
