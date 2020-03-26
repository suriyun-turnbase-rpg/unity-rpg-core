using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Clan : IClan
{
    public string id;
    public string Id { get { return id; } set { id = value; } }
    public string ownerId;
    public string OwnerId { get { return ownerId; } set { ownerId = value; } }
    public string name;
    public string Name { get { return name; } set { name = value; } }
    public string description;
    public string Description { get { return description; } set { description = value; } }
    public Player owner;
    public Player Owner { get { return owner; } set { owner = value; } }

    public Clan Clone()
    {
        return CloneTo(this, new Clan());
    }

    public static T CloneTo<T>(IClan from, T to) where T : IClan
    {
        to.Id = from.Id;
        to.OwnerId = from.OwnerId;
        to.Name = from.Name;
        to.Description = from.Description;
        to.Owner = from.Owner;
        return to;
    }
}
