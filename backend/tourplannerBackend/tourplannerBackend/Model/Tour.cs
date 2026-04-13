namespace tourplannerBackend.Model
{
    public class Tour
    {
        public int Id { get; set; }
        public required User User { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string FromLocation { get; set; }
        public required string ToLocation { get; set; }
        public required TransportType TransportType { get; set; }
        public int? Distance { get; set; }
        public int? EstimatedTime { get; set; }
        public string? RouteInformation { get; set; }
    }
}
