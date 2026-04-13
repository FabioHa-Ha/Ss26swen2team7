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
        [Range(1, 5, ErrorMessage = "Difficulty must be between 1 and 5.")]
        public required int DifficultyId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total distance must be greater than 0.")]
        public required int TotalDistance { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total time must be greater than 0.")]
        public required int TotalTime { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public required int Rating { get; set; }
    }

    public class TourLogUpdateDto
    {
        public DateTime? Date { get; set; }
        public string? Comment { get; set; }

        [Range(1, 5, ErrorMessage = "Difficulty must be between 1 and 5.")]
        public int? DifficultyId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total distance must be greater than 0.")]
        public int? TotalDistance { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total time must be greater than 0.")]
        public int? TotalTime { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
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
