using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Beam.Data
{
    public class BeamContext : DbContext
    {
        public BeamContext(DbContextOptions<BeamContext> contextOptions) : base(contextOptions) { }

        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<Ray> Rays { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Prism> Prisms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }

    public class Frequency
    {
        public int FrequencyId { get; set; }
        public string Name { get; set; }
        public List<Ray> Rays { get; set;}
    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<Ray> Rays { get; set; }
        public List<Prism> Prisms { get; set; }
    }

    public class Ray
    {
        public int RayId { get; set; }
        public string Text { get; set; }
        public int FrequencyId { get; set; }
        public Frequency Frequency { get; set; }
        public List<Prism> Prisms { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }

    public class Prism
    {
        public int PrismId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int RayId { get; set; }
        public Ray Ray { get; set; }
    }

}