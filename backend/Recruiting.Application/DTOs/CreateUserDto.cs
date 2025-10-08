using System.ComponentModel.DataAnnotations;

namespace Recruiting.Application.DTOs
{
    public class CreateUserDto
    {
        [Required, MinLength(2)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Candidate";

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}

