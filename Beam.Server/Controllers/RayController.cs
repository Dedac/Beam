using Beam.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Beam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RayController : ControllerBase
    {
        Data.BeamContext _context;
        public RayController(Data.BeamContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public List<RayItem> Rays()
        {
            return _context.Rays.Select(r => new RayItem()
            {
                RayId = r.RayId,
                Text = r.Text,
                PrismCount = 0,
                UserName = "billy"

            }
            ).ToList();
        }

    }
}