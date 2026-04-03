namespace tourplannerBackend.Model
{
    public class TourLog
    {
        public required int Id { get; set; }
        public required Tour Tour { get; set; }
        public required User User { get; set; }
        public required DateTime Date { get; set; }
        public string? Comment { get; set; }
        public required Difficulty Difficulty { get; set; }
        public required int TotalDistance { get; set; }
        public required int TotalTime { get; set; }
        public required int Rating { get; set; }
    }
}
