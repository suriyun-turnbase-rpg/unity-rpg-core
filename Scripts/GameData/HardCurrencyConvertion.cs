[System.Serializable]
public struct HardCurrencyConversion
{
    public int requireHardCurrency;
    public int receiveSoftCurrency;

    public string ToJson()
    {
        return "{\"requireHardCurrency\":" + requireHardCurrency + "," +
            "\"receiveSoftCurrency\":" + receiveSoftCurrency + "}";
    }
}
