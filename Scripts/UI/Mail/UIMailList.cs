public class UIMailList : UIDataItemList<UIMail, Mail>
{
    public override void Show()
    {
        base.Show();
        GetMailList();
    }

    private void GetMailList()
    {
        ClearListItems();
        GameInstance.GameService.GetMailList(OnGetMailListSuccess, OnGetMailListFail);
    }

    private void OnGetMailListSuccess(MailListResult result)
    {
        foreach (var entry in result.list)
        {
            var ui = SetListItem(entry.Id);
            ui.data = entry;
            ui.Show();
        }
    }

    private void OnGetMailListFail(string error)
    {
        GameInstance.Singleton.OnGameServiceError(error, GetMailList);
    }
}
