using System.ComponentModel.DataAnnotations;

namespace Beam.Shared
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "A username is required.")]
        [StringLength(32, MinimumLength = 3, ErrorMessage = "Usernames must be between 3 and 32 characters.")]
        [RegularExpression("^[A-Za-z0-9._-]+$", ErrorMessage = "Usernames may only contain letters, numbers, and the characters . _ -")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A password is required.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
