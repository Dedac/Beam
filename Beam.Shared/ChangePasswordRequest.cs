using System.ComponentModel.DataAnnotations;

namespace Beam.Shared
{
    public class ChangePasswordRequest
    {
        [Required(ErrorMessage = "Your current password is required.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "A new password is required.")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Passwords must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password.")]
        [Compare(nameof(NewPassword), ErrorMessage = "The passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
