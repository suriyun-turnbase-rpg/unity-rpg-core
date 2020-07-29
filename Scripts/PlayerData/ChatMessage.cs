using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChatMessage : IChatMessage
{
    public string id;
    public string Id { get => id; set => id = value; }
    public string playerId;
    public string PlayerId { get => playerId; set => playerId = value; }
    public string clanId;
    public string ClanId { get => clanId; set => clanId = value; }
    public string profileName;
    public string ProfileName { get => profileName; set => profileName = value; }
    public string clanName;
    public string ClanName { get => clanName; set => clanName = value; }
    public string message;
    public string Message { get => message; set => message = value; }
    public long timestamp;
    public long Timestamp { get => timestamp; set => timestamp = value; }

    public ChatMessage Clone()
    {
        return CloneTo(this, new ChatMessage());
    }

    public static T CloneTo<T>(IChatMessage from, T to) where T : IChatMessage
    {
        to.Id = from.Id;
        to.PlayerId = from.PlayerId;
        to.ClanId = from.ClanId;
        to.ProfileName = from.ProfileName;
        to.ClanName = from.ClanName;
        to.Message = from.Message;
        to.Timestamp = from.Timestamp;
        return to;
    }
}
