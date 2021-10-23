using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Beam.Data
{
    public static class Configure
    {
        public static void ConfigureData(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BeamContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
