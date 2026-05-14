using System.ComponentModel.DataAnnotations;

namespace tourplannerBackend.DTOs
{
    public class ContactCreateDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "First name must not be empty.")]
        public required string FirstName { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Last name must not be empty.")]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class ContactUpdateDto
    {
        [MinLength(1, ErrorMessage = "First name must not be empty.")]
        public string? FirstName { get; set; }

        [MinLength(1, ErrorMessage = "Last name must not be empty.")]
        public string? LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }
    }

    public class ContactResponseDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
