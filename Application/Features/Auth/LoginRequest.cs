using System.ComponentModel.DataAnnotations;

namespace Application.Features.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "EmailRequired")]
    [MaxLength(256, ErrorMessage = "MaxLengthError")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "PasswordRequired")]
    [MaxLength(128, ErrorMessage = "MaxLengthError")]
    public string Password { get; set; } = string.Empty;
}
