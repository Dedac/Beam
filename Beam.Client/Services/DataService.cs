using Beam.Shared;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Beam.Client.Services
{
    public class DataService
    {
        public IReadOnlyList<Frequency> Frequencies { get; private set; } 
        public IDictionary<int, List<Ray>> RayCollection { get; private set; }
        public User CurrentUser { get; set; }

        private readonly HttpClient http;
        public DataService(HttpClient httpInstance)
        {
            http = httpInstance;
        }

        public event Action UdpatedFrequencies;

        public async Task GetFrequencies()
        {
            Frequencies = await http.GetJsonAsync<List<Frequency>>("/api/Frequency/All");
            UdpatedFrequencies?.Invoke();
        }

        public async Task GetRays(int FrequencyId)
        {
            RayCollection[FrequencyId] = await http.GetJsonAsync<List<Ray>>($"/api/Rays/{FrequencyId}");
        }

        public async Task AddFrequency(string Name)
        {
            Frequencies = await http.PostJsonAsync<List<Frequency>>("/api/Frequency/Add", new Frequency() { Name = Name });
            UdpatedFrequencies?.Invoke();
        }

        public async Task CreateRay(int FrequencyId, Ray ray)
        {
            RayCollection[FrequencyId] = await http.PostJsonAsync<List<Ray>>("/api/Ray/Add", ray);
        }

        public async Task GetUser(string UserName)
        {
            CurrentUser = await http.GetJsonAsync<User>($"api/User/Get/{UserName}");
        }

        public async Task PrismRay(int RayId, int UserId, int FrequencyId)
        {
            RayCollection[FrequencyId] = await http.PostJsonAsync<List<Ray>>("/api/Prism/Add", new Prism() { RayId = RayId, UserId = UserId });
        }
    }
}
