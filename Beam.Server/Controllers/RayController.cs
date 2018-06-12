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
    public class RayController : ControllerBase
    {
        Data.BeamContext _context;
        public RayController(Data.BeamContext context)
        {
            _context = context;
        }
        [HttpGet("[action]/{username}")]
        public List<Ray> PrismRays(string username)
        {
            return _context.Rays.Include(r => r.Prisms).Include(r => r.User)
                .Where(r => r.Prisms.Any(p => p.User.Username == username))
                .Select(r => r.ToShared()).ToList();
        }


        [HttpGet("{FrequencyId}")]
        public List<Ray> Rays(int FrequencyId)
        {
            return GetRays(FrequencyId);
        }

        private List<Ray> GetRays(int FrequencyId)
        {
            return _context.Rays.Include(r => r.Prisms).Include(r => r.User)
                .Where(r => r.FrequencyId == FrequencyId)
                .Select(r => r.ToShared()).ToList();
        }

        [HttpPost("[action]")]
        public List<Ray> Add([FromBody] Ray ray)
        {
            _context.Add(ray.ToData());
            _context.SaveChanges();
            return GetRays(ray.FrequencyId);
        }

    }
}