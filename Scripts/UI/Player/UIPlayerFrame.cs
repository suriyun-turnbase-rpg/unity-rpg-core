using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerFrame : MonoBehaviour
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

        if (_id != Player.FrameId || (!_setDefaultOnce && string.IsNullOrWhiteSpace(Player.FrameId)))
        {
            _id = Player.FrameId;

            GenericUnlockable data;
            if (string.IsNullOrWhiteSpace(Player.FrameId) || !GameInstance.GameDatabase.PlayerFrames.TryGetValue(Player.FrameId, out data))
            {
                data = GameInstance.GameDatabase.PlayerFrames.Values.FirstOrDefault();
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
