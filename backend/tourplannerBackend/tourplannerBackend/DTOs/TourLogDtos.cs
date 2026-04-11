using System.ComponentModel.DataAnnotations;

namespace tourplannerBackend.DTOs
{
    public class TourLogCreateDto
    {
        [Required]
        public required int TourId { get; set; }

        [Required]
        public required DateTime Date { get; set; }

        public string? Comment { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Schwierigkeitsgrad muss zwischen 1 und 5 liegen.")]
        public required int DifficultyId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Gesamtstrecke muss größer als 0 sein.")]
        public required int TotalDistance { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Gesamtzeit muss größer als 0 sein.")]
        public required int TotalTime { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Bewertung muss zwischen 1 und 5 liegen.")]
        public required int Rating { get; set; }
    }

    public class TourLogUpdateDto
    {
        public DateTime? Date { get; set; }
        public string? Comment { get; set; }

        [Range(1, 5, ErrorMessage = "Schwierigkeitsgrad muss zwischen 1 und 5 liegen.")]
        public int? DifficultyId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Gesamtstrecke muss größer als 0 sein.")]
        public int? TotalDistance { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Gesamtzeit muss größer als 0 sein.")]
        public int? TotalTime { get; set; }

        [Range(1, 5, ErrorMessage = "Bewertung muss zwischen 1 und 5 liegen.")]
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
