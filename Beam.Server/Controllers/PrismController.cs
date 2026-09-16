using Beam.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Beam.Server.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Beam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrismController : ControllerBase
    {
        Data.BeamContext _context;
        public PrismController(Data.BeamContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("[action]")]
        public ActionResult<List<Ray>> Add([FromBody] Prism prism)
        {
            var userId = User.GetUserId();

            if (userId == null) return Unauthorized();

            var prismRay = _context.Rays.Find(prism.RayId);

            if (prismRay == null) return new List<Ray>();

            var alreadyPrismed = _context.Prisms.Any(p => p.RayId == prism.RayId && p.UserId == userId.Value);

            if (!alreadyPrismed)
            {
                _context.Add(new Data.Prism { RayId = prism.RayId, UserId = userId.Value });
                _context.SaveChanges();
            }

            return _context.Rays.Include(r => r.Prisms).ThenInclude(p => p.User).Include(r => r.User)
                .Where(r => r.FrequencyId == prismRay.FrequencyId)
                .Select(r => r.ToShared())
                .ToList();
        }

        [Authorize]
        [HttpGet("[action]/{RayId}")]
        public ActionResult<List<Ray>> Remove(int RayId)
        {
            var userId = User.GetUserId();

            if (userId == null) return Unauthorized();

            var removePrisms = _context.Prisms.Include(p => p.Ray).Where(p => p.RayId == RayId && p.UserId == userId.Value).ToList();
            if (removePrisms.Count <= 0) return new List<Ray>();

            var frequencyId = removePrisms.First().Ray.FrequencyId;
            _context.RemoveRange(removePrisms);

            _context.SaveChanges();

            return _context.Rays.Include(r => r.Prisms).ThenInclude(p => p.User).Include(r => r.User)
                .Where(r => r.FrequencyId == frequencyId)
                .Select(r => r.ToShared())
                .ToList();
        }

    }
}