using UnityEngine;

public class GenericUnlockable : BaseGameData
{
    [Tooltip("It will be unlocked by default, so set this to `TRUE` to locked by default to be unlocked later")]
    public bool locked;

    public virtual string ToJson()
    {
        return "{\"id\":\"" + Id + "\"," +
            "\"isLocking\":" + (locked ? "true" : "false") + "}";
    }
}
