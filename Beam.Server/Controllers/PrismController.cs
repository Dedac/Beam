﻿using Beam.Shared;
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

        [HttpPost("[action]")]
        public List<Ray> Add([FromBody] Prism prism)
        {
            var newPrism = prism.ToData();

            _context.Add(newPrism);
            _context.SaveChanges();

            var prismRay = _context.Rays.Find(newPrism.RayId);

            return _context.Rays.Include(r => r.Prisms).Include(r => r.User)
                .Where(r => r.FrequencyId == prismRay.FrequencyId)
                .Select(r => r.ToShared())
                .ToList();
        }

        [HttpPost("[action]")]
        public List<Ray> Remove([FromBody] int prismId)
        {
            var removePrism = _context.Find<Data.Prism>(prismId);
            var frequencyId = removePrism.Ray.FrequencyId;
            _context.Remove(removePrism);
            
            _context.SaveChanges();

            return _context.Rays.Include(r => r.Prisms).Include(r => r.User)
                .Where(r => r.FrequencyId == frequencyId)
                .Select(r => r.ToShared())
                .ToList();
        }

    }
}