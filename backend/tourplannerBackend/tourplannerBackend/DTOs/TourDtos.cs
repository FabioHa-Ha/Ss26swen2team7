namespace tourplannerBackend.DTOs
{
    public class TourCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string FromLocation { get; set; }
        public required string ToLocation { get; set; }
        public required int TransportTypeId { get; set; }
        public int? Distance { get; set; }
        public int? EstimatedTime { get; set; }
        public string? RouteInformation { get; set; }
    }

    public class TourUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public int? TransportTypeId { get; set; }
        public int? Distance { get; set; }
        public int? EstimatedTime { get; set; }
        public string? RouteInformation { get; set; }
    }

    public class TourResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string FromLocation { get; set; }
        public required string ToLocation { get; set; }
        public int TransportTypeId { get; set; }
        public string? TransportTypeName { get; set; }
        public int? Distance { get; set; }
        public int? EstimatedTime { get; set; }
        public string? RouteInformation { get; set; }
    }
}
