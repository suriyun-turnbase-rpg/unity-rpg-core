using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class UIGenericUnlockableEvent : UnityEvent<UIGenericUnlockable> { }

public class UIGenericUnlockable : UIDataItem<GenericUnlockable>
{
    public Text textTitle;
    public Text textDescription;
    public Image imageIcon;
    public Image imageIcon2;
    public Image imageIcon3;
    public GameObject lockObject;
    public GameObject unlockObject;
    public System.Func<GenericUnlockable, bool> unlockDetectFunction;

    protected override void Update()
    {
        base.Update();

        if (unlockDetectFunction != null)
        {
            if (lockObject != null)
                lockObject.SetActive(!unlockDetectFunction.Invoke(data));

            if (unlockObject != null)
                unlockObject.SetActive(unlockDetectFunction.Invoke(data));
        }
    }

    public override void UpdateData()
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

    public override void Clear()
    {
        if (textTitle != null)
            textTitle.text = string.Empty;

        if (textDescription != null)
            textDescription.text = string.Empty;

        if (imageIcon != null)
            imageIcon.sprite = null;

        if (imageIcon2 != null)
            imageIcon2.sprite = null;

        if (imageIcon3 != null)
            imageIcon3.sprite = null;
    }

    public override bool IsEmpty()
    {
        return data == null;
    }
}
