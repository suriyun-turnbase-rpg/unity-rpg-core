using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerIcon : MonoBehaviour
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

        if (_id != Player.IconId || (!_setDefaultOnce && string.IsNullOrWhiteSpace(Player.IconId)))
        {
            _id = Player.IconId;

            GenericUnlockable data;
            if (string.IsNullOrWhiteSpace(Player.IconId) || !GameInstance.GameDatabase.PlayerIcons.TryGetValue(Player.IconId, out data))
            {
                data = GameInstance.GameDatabase.PlayerIcons.Values.FirstOrDefault();
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
