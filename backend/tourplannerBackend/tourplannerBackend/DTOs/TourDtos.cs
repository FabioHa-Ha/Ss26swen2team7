using System.ComponentModel.DataAnnotations;

namespace tourplannerBackend.DTOs
{
    public class TourCreateDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Name must not be empty.")]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Start location must not be empty.")]
        public required string FromLocation { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Destination must not be empty.")]
        public required string ToLocation { get; set; }

        [Required]
        public required int TransportTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Distance must be greater than 0.")]
        public int? Distance { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Estimated time must be greater than 0.")]
        public int? EstimatedTime { get; set; }

        public string? RouteInformation { get; set; }
        public int? ImageId { get; set; }
    }

    public class TourUpdateDto
    {
        [MinLength(1, ErrorMessage = "Name must not be empty.")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [MinLength(1, ErrorMessage = "Start location must not be empty.")]
        public string? FromLocation { get; set; }

        [MinLength(1, ErrorMessage = "Destination must not be empty.")]
        public string? ToLocation { get; set; }

        public int? TransportTypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Distance must be greater than 0.")]
        public int? Distance { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Estimated time must be greater than 0.")]
        public int? EstimatedTime { get; set; }

        public string? RouteInformation { get; set; }
        public int? ImageId { get; set; }
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
        public int? ImageId { get; set; }
    }
}
