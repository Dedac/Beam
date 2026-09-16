using System.Globalization;
using System.Security.Claims;
using Beam.Server.Mappers;
using Beam.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Beam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Data.BeamContext _context;
        private readonly IPasswordHasher<Data.User> _passwordHasher;

        public AuthController(Data.BeamContext context, IPasswordHasher<Data.User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<AuthResult>> Register([FromBody] RegisterRequest request)
        {
            var username = request.Username.Trim();

            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                return Conflict(AuthResult.Failure("That username is already taken."));
            }

            var user = new Data.User { Username = username };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(AuthResult.Failure("That username is already taken."));
            }

            await SignInAsync(user);

            return AuthResult.Success(user.ToShared());
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<AuthResult>> Login([FromBody] LoginRequest request)
        {
            var username = request.Username.Trim();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                return Unauthorized(AuthResult.Failure("Incorrect username or password."));
            }

            var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verification == PasswordVerificationResult.Failed)
            {
                return Unauthorized(AuthResult.Failure("Incorrect username or password."));
            }

            if (verification == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
                await _context.SaveChangesAsync();
            }

            await SignInAsync(user);

            return AuthResult.Success(user.ToShared());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<User>> Me()
        {
            var userId = User.GetUserId();

            if (userId == null)
            {
                return NoContent();
            }

            var user = await _context.Users.FindAsync(userId.Value);

            if (user == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return NoContent();
            }

            return user.ToShared();
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult<AuthResult>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.GetUserId();
            var user = userId == null ? null : await _context.Users.FindAsync(userId.Value);

            if (user == null)
            {
                return Unauthorized(AuthResult.Failure("You are no longer signed in."));
            }

            var verification = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? string.Empty, request.CurrentPassword);

            if (verification == PasswordVerificationResult.Failed)
            {
                return BadRequest(AuthResult.Failure("Your current password is incorrect."));
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            await _context.SaveChangesAsync();

            await SignInAsync(user);

            return AuthResult.Success(user.ToShared());
        }

        private async Task SignInAsync(Data.User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = true });
        }
    }
}
