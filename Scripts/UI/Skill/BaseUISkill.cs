using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class BaseUISkill<TSkill> : UIDataItem<TSkill>
    where TSkill : BaseSkill
{
    public Text textTitle;
    public Text textDescription;
    public Image imageIcon;
    public Image imageIcon2;
    public Image imageIcon3;

    public override void Clear()
    {
        SetupInfo(null);
    }

    public override void UpdateData()
    {
        SetupInfo(data);
    }

    private void SetupInfo(TSkill data)
    {
        if (textTitle != null)
            textTitle.text = data == null ? "" : data.Title;

        if (textDescription != null)
            textDescription.text = data == null ? "" : data.Description;

        if (imageIcon != null)
        {
            imageIcon.sprite = data == null ? null : data.icon;
            imageIcon.preserveAspect = true;
        }

        if (imageIcon2 != null)
        {
            imageIcon2.sprite = data == null ? null : data.icon2;
            imageIcon2.preserveAspect = true;
        }

        if (imageIcon3 != null)
        {
            imageIcon3.sprite = data == null ? null : data.icon3;
            imageIcon3.preserveAspect = true;
        }
    }

    public override bool IsEmpty()
    {
        return data == null || string.IsNullOrEmpty(data.Id);
    }

    public void ShowDataOnMessageDialog()
    {
        GameInstance.Singleton.ShowMessageDialog(data.Title, data.Description);
    }
}
