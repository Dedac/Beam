using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Beam.Shared;
using Microsoft.AspNetCore.Components;

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
            return http.GetFromJsonAsync<List<Frequency>>("api/Frequency/All");
        }

        internal Task<List<Ray>> RayList(int frequencyId)
        {
            return http.GetFromJsonAsync<List<Ray>>($"api/Ray/{frequencyId}");
        }

        internal async Task<List<Frequency>> AddFrequency(Frequency frequency)
        {
            var resp = await http.PostAsJsonAsync("api/Frequency/Add", frequency);
            return await resp.Content.ReadFromJsonAsync<List<Frequency>>();
        }

        internal async Task<List<Ray>> AddRay(Ray ray)
        {
            var resp = await http.PostAsJsonAsync("api/Ray/Add", ray);
            return await resp.Content.ReadFromJsonAsync<List<Ray>>();
        }

        internal Task<User> GetOrCreateUser(string name)
        {
            return http.GetFromJsonAsync<User>($"api/User/Get/{name}");
        }

        internal async Task<List<Ray>> PrismRay(Prism prism)
        {
            var resp = await http.PostAsJsonAsync("api/Prism/Add", prism);
            return await resp.Content.ReadFromJsonAsync<List<Ray>>();           
        }

        internal Task<List<Ray>> UnPrismRay(int rayId, int userId)
        {
            return http.GetFromJsonAsync<List<Ray>>($"api/Prism/Remove/{userId}/{rayId}");
        }

        internal Task<List<Ray>> UserRays(string name)
        {
            return http.GetFromJsonAsync<List<Ray>>($"api/Ray/user/{name}");
        }
    }
}
