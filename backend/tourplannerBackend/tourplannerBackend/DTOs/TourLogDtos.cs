namespace tourplannerBackend.DTOs
{
    public class TourLogCreateDto
    {
        public required int TourId { get; set; }
        public required DateTime Date { get; set; }
        public string? Comment { get; set; }
        public required int DifficultyId { get; set; }
        public required int TotalDistance { get; set; }
        public required int TotalTime { get; set; }
        public required int Rating { get; set; }
    }

    public class TourLogUpdateDto
    {
        public DateTime? Date { get; set; }
        public string? Comment { get; set; }
        public int? DifficultyId { get; set; }
        public int? TotalDistance { get; set; }
        public int? TotalTime { get; set; }
        public int? Rating { get; set; }
    }

    public class TourLogResponseDto
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public int DifficultyId { get; set; }
        public string? DifficultyName { get; set; }
        public int TotalDistance { get; set; }
        public int TotalTime { get; set; }
        public int Rating { get; set; }
    }
}
