using UnityEngine.UI;

public class UIMail : UIDataItem<Mail>
{
    public Text textTitle;
    public Text textContent;
    public Text textSendTimestamp;
    public UIGameDataWithAmountList uiRewards;

    public override void Clear()
    {
        // Don't clear
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(Mail data)
    {
        if (textTitle != null)
            textTitle.text = data.Title;

        if (textContent != null)
            textContent.text = data.Content;

        if (textSendTimestamp != null)
        {
            var d = new System.DateTime(1970, 1, 1);
            d = d.AddSeconds(data.SentTimestamp);
            textSendTimestamp.text = d.GetPrettyDate();
        }

        if (uiRewards != null)
        {

        }
    }
}
