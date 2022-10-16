public partial interface IClan
{
    string Id { get; set; }
    int Exp { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    Player Owner { get; set; }
    string IconId { get; set; }
    string FrameId { get; set; }
    string TitleId { get; set; }
}
