public partial interface IRaidEvent
{
    string Id { get; set; }
    string DataId { get; set; }
    int RemainingHp { get; set; }
    int StartTime { get; set; }
    int EndTime { get; set; }
}
