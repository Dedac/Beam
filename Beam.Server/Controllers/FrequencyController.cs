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
        public List<Frequency> All()
        {
            return GetAll();
        }

        [HttpPost("[action]")]
        public List<Frequency> Add([FromBody] Frequency frequency)
        {
            _context.Add(new Data.Frequency() { Name = frequency.Name });
            _context.SaveChanges();
            return GetAll();
        }

        private List<Frequency> GetAll()
        {
            return _context.Frequencies.Select(r => new Frequency()
            {
                Id = r.FrequencyId,
                Name = r.Name,
            }
            ).ToList();

        }
    }
}