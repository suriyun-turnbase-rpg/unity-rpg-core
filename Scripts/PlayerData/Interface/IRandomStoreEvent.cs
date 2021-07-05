public partial interface IRandomStoreEvent
{
    string DataId { get; set; }
    string RandomedItems { get; set; }
    string PurchaseItems { get; set; }
}
