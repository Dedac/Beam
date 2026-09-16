using System.Globalization;
using System.Security.Claims;

namespace Beam.Server
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            var value = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
