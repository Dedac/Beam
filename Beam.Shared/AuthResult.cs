namespace Beam.Shared
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public User User { get; set; }

        public static AuthResult Success(User user) => new AuthResult { Succeeded = true, User = user };
        public static AuthResult Failure(string error) => new AuthResult { Succeeded = false, Error = error };
    }
}
