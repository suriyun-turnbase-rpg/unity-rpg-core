using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Clan : IClan
{
    public string id;
    public string Id { get { return id; } set { id = value; } }
    public string name;
    public string Name { get { return name; } set { name = value; } }

    public Clan Clone()
    {
        return CloneTo(this, new Clan());
    }

    public static T CloneTo<T>(IClan from, T to) where T : IClan
    {
        to.Id = from.Id;
        to.Name = from.Name;
        return to;
    }
}
