using Beam.Server.Mappers;
using Beam.Shared;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System.Linq;

namespace Beam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        Data.BeamContext _context;
        public UserController(Data.BeamContext context)
        {
            _context = context;
        }

        [HttpGet("[action]/{Username}")]
        public async Task<Shared.User> Get(string Username)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == Username);

            if (existingUser != null)
            {
                return existingUser.ToShared();
            }
            
            var newUser = new Data.User() { Username = Username };

            var ghUser = await RetrieveGitHubUser(Username);
            if (ghUser != null) {
                newUser.Rays = new List<Data.Ray>();
                newUser.Rays.Add(new Data.Ray() {
                    FrequencyId = 1,
                    Text = ghUser.Bio
                });
            }

            _context.Add(newUser);
            _context.SaveChanges();

            return newUser.ToShared();
        }

        private async static Task<Octokit.User?> RetrieveGitHubUser(string Username)
        {
            //Altered so it isn't "real"
            var GHAccess = "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";
            // get user from github api and return user object
            var GHClient = new GitHubClient(new ProductHeaderValue("Beam"));
            GHClient.Credentials = new Credentials(GHAccess);
            var limits = await GHClient.Miscellaneous.GetRateLimits();
            if (limits?.Rate?.Remaining == null || limits.Rate.Remaining > 0)
            {
                var ghUser = await GHClient.User.Get(Username);
                return ghUser;
            }
            return null;
        }
    }
}