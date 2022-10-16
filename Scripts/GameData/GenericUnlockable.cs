public class GenericUnlockable : BaseGameData
{
    public bool isLocking;

    public virtual string ToJson()
    {
        return "{\"id\":\"" + Id + "\"," +
            "\"isLocking\":" + (isLocking ? "true" : "false") + "}";
    }
}
