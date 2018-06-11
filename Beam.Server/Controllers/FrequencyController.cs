using Beam.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Beam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequencyController : ControllerBase
    {
        Data.BeamContext _context;
        public FrequencyController(Data.BeamContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public List<FrequencyItem> All()
        {
            return _context.Frequencies.Select(r => new FrequencyItem()
            {
                Id = r.FrequencyId,
                Name = r.Name,
            }
            ).ToList();
        }
    }
}