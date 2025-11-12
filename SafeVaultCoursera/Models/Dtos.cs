using System.ComponentModel.DataAnnotations;

namespace SafeVaultCoursera.Models
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9_.-]*$", ErrorMessage = "Username contém caracteres inválidos.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
