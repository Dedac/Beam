using System.Collections.Generic;
using System.Threading.Tasks;
using Beam.Shared;

namespace Beam.Client.Services
{
    public interface IBeamApiService
    {
        Task<List<Ray>> GetUserRays(string name);
       
        Task<List<Frequency>> FrequencyList();
       
        Task<List<Ray>> RayList(int frequencyId);
        
        Task<List<Frequency>> AddFrequency(Frequency frequency);
        
        Task<List<Ray>> AddRay(Ray ray);
        
        Task<List<Ray>> PrismRay(Prism prism);
        
        Task<List<Ray>> UnPrismRay(int rayId, int userId);
        
        Task<User> GetUser();

        Task Login(Login login);

        Task Register(Login login);

        Task Logout();
   }
}