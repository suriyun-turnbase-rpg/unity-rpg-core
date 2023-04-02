using UnityEngine.UI;

public class UIArenaRank : UIDataItem<ArenaRank>
{
    public Text textTitle;
    public Text textDescription;
    public Image imageIcon;
    public Image imageIcon2;
    public Image imageIcon3;
    public Image imageHighlight;

    public override void Clear()
    {
        SetupInfo(null);
    }

    private void SetupInfo(ArenaRank data)
    {
        if (data == null)
            return;

        if (textTitle != null)
            textTitle.text = data.Title;

        if (textDescription != null)
            textDescription.text = data.Description;

        if (imageIcon != null)
        {
            imageIcon.sprite = data.icon;
            imageIcon.preserveAspect = true;
        }

        if (imageIcon2 != null)
        {
            imageIcon2.sprite = data.icon2;
            imageIcon2.preserveAspect = true;
        }

        if (imageIcon3 != null)
        {
            imageIcon3.sprite = data.icon3;
            imageIcon3.preserveAspect = true;
        }

        if (imageHighlight != null)
        {
            imageHighlight.sprite = data.highlight;
            imageHighlight.preserveAspect = true;
        }
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }
}
