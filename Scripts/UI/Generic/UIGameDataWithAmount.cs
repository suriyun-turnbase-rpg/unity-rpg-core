using UnityEngine.UI;

public class UIGameDataWithAmount : UIDataItem<IGameData>
{
    public Text textAmount;
    public Image imageIcon;
    public Image imageIcon2;
    public Image imageIcon3;

    public int Amount { get; set; }

    protected override void Update()
    {
        base.Update();
        if (textAmount != null)
            textAmount.text = data == null ? string.Empty : Amount.ToString("N0");
    }

    public override void Clear()
    {
        SetupInfo(null);
        Amount = 0;
    }

    public override bool IsEmpty()
    {
        return data == null;
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(IGameData data)
    {
        if (imageIcon != null)
        {
            imageIcon.sprite = data == null ? null : data.Icon;
            imageIcon.preserveAspect = true;
        }

        if (imageIcon2 != null)
        {
            imageIcon2.sprite = data == null ? null : data.Icon2;
            imageIcon2.preserveAspect = true;
        }

        if (imageIcon3 != null)
        {
            imageIcon3.sprite = data == null ? null : data.Icon3;
            imageIcon3.preserveAspect = true;
        }
    }
}
