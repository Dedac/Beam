using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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

        internal async Task<List<Frequency>?> AddFrequency(Frequency frequency)
        {
            var resp = await http.PostAsJsonAsync("api/Frequency/Add", frequency);

            if (!resp.IsSuccessStatusCode) return null;

            return await resp.Content.ReadFromJsonAsync<List<Frequency>>();
        }

        internal async Task<List<Ray>> AddRay(Ray ray)
        {
            var resp = await http.PostAsJsonAsync("api/Ray/Add", ray);

            if (!resp.IsSuccessStatusCode) return new List<Ray>();

            return (await resp.Content.ReadFromJsonAsync<List<Ray>>()) ?? new List<Ray>();
        }

        internal async Task<User?> GetUser(string name)
        {
            var resp = await http.GetAsync($"api/User/Get/{Uri.EscapeDataString(name)}");

            if (!resp.IsSuccessStatusCode) return null;

            return await resp.Content.ReadFromJsonAsync<User>();
        }

        internal async Task<AuthResult> Register(RegisterRequest request)
        {
            return await ReadAuthResult(await http.PostAsJsonAsync("api/Auth/Register", request));
        }

        internal async Task<AuthResult> Login(LoginRequest request)
        {
            return await ReadAuthResult(await http.PostAsJsonAsync("api/Auth/Login", request));
        }

        internal async Task<AuthResult> ChangePassword(ChangePasswordRequest request)
        {
            return await ReadAuthResult(await http.PostAsJsonAsync("api/Auth/ChangePassword", request));
        }

        internal async Task Logout()
        {
            await http.PostAsync("api/Auth/Logout", null);
        }

        internal async Task<User?> CurrentUser()
        {
            var resp = await http.GetAsync("api/Auth/Me");

            if (!resp.IsSuccessStatusCode || resp.StatusCode == HttpStatusCode.NoContent) return null;

            try
            {
                return await resp.Content.ReadFromJsonAsync<User>();
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static async Task<AuthResult> ReadAuthResult(HttpResponseMessage response)
        {
            try
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResult>();

                if (result != null && (result.Succeeded || !string.IsNullOrWhiteSpace(result.Error)))
                {
                    return result;
                }
            }
            catch (JsonException)
            {
                // The server returned a payload we cannot interpret; fall through to a generic message.
            }

            if (response.IsSuccessStatusCode) return AuthResult.Failure("The server sent an unexpected response.");

            return AuthResult.Failure("Please check the highlighted fields and try again.");
        }

        internal async Task<List<Ray>> PrismRay(Prism prism)
        {
            var resp = await http.PostAsJsonAsync("api/Prism/Add", prism);

            if (!resp.IsSuccessStatusCode) return new List<Ray>();

            return (await resp.Content.ReadFromJsonAsync<List<Ray>>()) ?? new List<Ray>();
        }

        internal async Task<List<Ray>> UnPrismRay(int rayId)
        {
            var resp = await http.GetAsync($"api/Prism/Remove/{rayId}");

            if (!resp.IsSuccessStatusCode) return new List<Ray>();

            return (await resp.Content.ReadFromJsonAsync<List<Ray>>()) ?? new List<Ray>();
        }

        internal async Task<List<Ray>> UserRays(string name)
        {
            return (await http.GetFromJsonAsync<List<Ray>>($"api/Ray/user/{name}")) ?? new List<Ray>();
        }
    }
}
