using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerTitle : MonoBehaviour
{
    public Image imageIcon;
    public Image imageIcon2;
    public Image imageIcon3;
    public bool loadFromOwnedPlayer;

    public Player Player { get; set; }
    private string _id;
    private bool _setDefaultOnce;

    void Update()
    {
        if (loadFromOwnedPlayer)
            Player = Player.CurrentPlayer;

        if (Player == null)
            return;

        if (_id != Player.TitleId || (!_setDefaultOnce && string.IsNullOrWhiteSpace(Player.TitleId)))
        {
            _id = Player.TitleId;

            GenericUnlockable data;
            if (string.IsNullOrWhiteSpace(Player.TitleId) || !GameInstance.GameDatabase.PlayerTitles.TryGetValue(Player.TitleId, out data))
            {
                data = GameInstance.GameDatabase.PlayerTitles.Values.FirstOrDefault();
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
