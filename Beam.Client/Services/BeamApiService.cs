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

        internal async Task<List<Frequency>> FrequencyList()
        {
            return (await http.GetFromJsonAsync<List<Frequency>>("api/Frequency/All")) ?? new List<Frequency>();
        }

        internal async Task<List<Ray>> RayList(int frequencyId)
        {
            return (await http.GetFromJsonAsync<List<Ray>>($"api/Ray/{frequencyId}")) ?? new List<Ray>();
        }

        internal async Task<List<Frequency>> AddFrequency(Frequency frequency)
        {
            var resp = await http.PostAsJsonAsync("api/Frequency/Add", frequency);
            return (await resp.Content.ReadFromJsonAsync<List<Frequency>>()) ?? new List<Frequency>();
        }

        internal async Task<List<Ray>> AddRay(Ray ray)
        {
            var resp = await http.PostAsJsonAsync("api/Ray/Add", ray);
            return (await resp.Content.ReadFromJsonAsync<List<Ray>>()) ?? new List<Ray>();
        }

        internal async Task<User> GetOrCreateUser(string name)
        {
            return (await http.GetFromJsonAsync<User>($"api/User/Get/{name}")) ?? new User();
        }

        internal async Task<List<Ray>> PrismRay(Prism prism)
        {
            var resp = await http.PostAsJsonAsync("api/Prism/Add", prism);
            return (await resp.Content.ReadFromJsonAsync<List<Ray>>()) ?? new List<Ray>();           
        }

        internal async Task<List<Ray>> UnPrismRay(int rayId, int userId)
        {
            return (await http.GetFromJsonAsync<List<Ray>>($"api/Prism/Remove/{userId}/{rayId}")) ?? new List<Ray>();
        }

        internal async Task<List<Ray>> UserRays(string name)
        {
            return (await http.GetFromJsonAsync<List<Ray>>($"api/Ray/user/{name}")) ?? new List<Ray>();
        }
    }
}
