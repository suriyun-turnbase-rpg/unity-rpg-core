[System.Serializable]
public struct HardCurrencyConvertion
{
    public int requireHardCurrency;
    public int receiveSoftCurrency;

    public string ToJson()
    {
        return "{\"requireHardCurrency\":" + requireHardCurrency + "," +
            "\"receiveSoftCurrency\":" + receiveSoftCurrency + "}";
    }
}
