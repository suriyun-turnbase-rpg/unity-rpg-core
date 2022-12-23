using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIClanTitle : MonoBehaviour
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

        if (_id != Clan.TitleId || (!_setDefaultOnce && string.IsNullOrWhiteSpace(Clan.TitleId)))
        {
            _id = Clan.TitleId;

            GenericUnlockable data;
            if (string.IsNullOrWhiteSpace(Clan.TitleId) || !GameInstance.GameDatabase.ClanTitles.TryGetValue(Clan.TitleId, out data))
            {
                data = GameInstance.GameDatabase.ClanTitles.Values.FirstOrDefault();
                _setDefaultOnce = true;
            }

            if (imageIcon != null)
                imageIcon.sprite = data == null ? null : data.icon;

            if (imageIcon2 != null)
                imageIcon2.sprite = data == null ? null : data.icon2;

            if (imageIcon3 != null)
                imageIcon3.sprite = data == null ? null : data.icon3;
        }
    }
}
