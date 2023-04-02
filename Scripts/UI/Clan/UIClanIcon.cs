using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIClanIcon : MonoBehaviour
{
    public Image imageIcon;
    public Image imageIcon2;
    public Image imageIcon3;

    public Clan Clan { get; set; }
    private string _id;
    private bool _setDefaultOnce;

    void Update()
    {
        if (Clan == null)
            return;

        if (_id != Clan.IconId || (!_setDefaultOnce && string.IsNullOrWhiteSpace(Clan.IconId)))
        {
            _id = Clan.IconId;

            GenericUnlockable data;
            if (string.IsNullOrWhiteSpace(Clan.IconId) || !GameInstance.GameDatabase.ClanIcons.TryGetValue(Clan.IconId, out data))
            {
                data = GameInstance.GameDatabase.ClanIcons.Values.FirstOrDefault();
                _setDefaultOnce = true;
            }

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
    }
}
