using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Beam.Shared;
using Microsoft.AspNetCore.Blazor;

namespace Beam.Client.Services
{
    public class BeamApiService
    {
        HttpClient http;
        public BeamApiService(HttpClient httpInstance)
        {
            http = httpInstance;
        }

        internal Task<List<Frequency>> FrequencyList()
        {
            return http.GetJsonAsync<List<Frequency>>("/api/Frequency/All");
        }

        internal Task<List<Ray>> RayList(int frequencyId)
        {
            return http.GetJsonAsync<List<Ray>>($"/api/Ray/{frequencyId}");
        }

        internal Task<List<Frequency>> AddFrequency(Frequency frequency)
        {
            return http.PostJsonAsync<List<Frequency>>("/api/Frequency/Add", frequency);
        }

        internal Task<List<Ray>> AddRay(Ray ray)
        {
            return http.PostJsonAsync<List<Ray>>("/api/Ray/Add", ray);
        }

        internal Task<User> GetOrCreateUser(string name)
        {
            return http.GetJsonAsync<User>($"api/User/Get/{name}");
        }

        internal Task<List<Ray>> PrismRay(Prism prism)
        {
            return http.PostJsonAsync<List<Ray>>("/api/Prism/Add", prism);
        }

        internal Task<List<Ray>> UnPrismRay(int rayId, int userId)
        {
            return http.GetJsonAsync<List<Ray>>($"/api/Prism/Remove/{userId}/{rayId}");
        }

        internal Task<List<Ray>> UserRays(string name)
        {
            return http.GetJsonAsync<List<Ray>>($"/api/Ray/user/{name}");
        }
    }
}
