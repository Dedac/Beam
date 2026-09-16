using System.ComponentModel.DataAnnotations;

namespace Beam.Shared
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "A username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "A password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
