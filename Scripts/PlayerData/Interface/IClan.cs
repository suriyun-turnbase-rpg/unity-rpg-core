public partial interface IClan
{
    string Id { get; set; }
    string OwnerId { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    IPlayer Owner { get; set; }
}
