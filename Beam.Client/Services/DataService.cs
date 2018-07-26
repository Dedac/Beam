using Beam.Shared;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Beam.Client.Services
{
    public class DataService
    {
        private readonly HttpClient http;
        public IReadOnlyList<Frequency> Frequencies { get; private set; }
        public IReadOnlyList<Ray> Rays { get; private set; } = new List<Ray>();
        public User CurrentUser { get; set; }

        private int? selectedFrequency;
        public int SelectedFrequency
        {
            get
            {
                if (!selectedFrequency.HasValue && Frequencies.Count > 0)
                {
                    selectedFrequency = Frequencies.First().Id;
                }
                return selectedFrequency ?? 0;
            }
            set
            {
                selectedFrequency = value;
                GetRays(value).ConfigureAwait(false);
            }
        }

        public DataService(HttpClient httpInstance)
        {
            http = httpInstance;
            if (CurrentUser == null) CurrentUser = new User() { Name = "Anon" + new Random().Next(0, 10) };
        }

        public event Action UdpatedFrequencies;
        public event Action UpdatedRays;

        public async Task GetFrequencies()
        {
            Frequencies = await http.GetJsonAsync<List<Frequency>>("/api/Frequency/All");
            UdpatedFrequencies?.Invoke();
        }

        public async Task GetRays(int FrequencyId)
        {
            Rays = new List<Ray>();
            Rays = await http.GetJsonAsync<List<Ray>>($"/api/Ray/{FrequencyId}");
            UpdatedRays?.Invoke();
        }

        public async Task AddFrequency(string Name)
        {
            Frequencies = await http.PostJsonAsync<List<Frequency>>("/api/Frequency/Add", new Frequency() { Name = Name });
            UdpatedFrequencies?.Invoke();
        }

        public async Task CreateRay(string text)
        {
            var ray = new Ray()
            {
                FrequencyId = selectedFrequency.Value,
                Text = text,
                UserId = CurrentUser.Id
            };

            if (CurrentUser.Id == 0)
            {
                await GetOrCreateUser();
                ray.UserId = CurrentUser.Id;
            }

            Rays = await http.PostJsonAsync<List<Ray>>("/api/Ray/Add", ray);
            UpdatedRays?.Invoke();
        }

        public async Task GetOrCreateUser(string newName = null)
        {
            CurrentUser = await http.GetJsonAsync<User>($"api/User/Get/{newName ?? CurrentUser.Name}");
        }

        public async Task PrismRay(int RayId)
        {
            if (CurrentUser.Id == 0) await GetOrCreateUser();
            Rays = await http.PostJsonAsync<List<Ray>>("/api/Prism/Add", new Prism() { RayId = RayId, UserId = CurrentUser.Id });
            UpdatedRays?.Invoke();
        }

        public async Task UnPrismRay(int RayId)
        {
            if (CurrentUser.Id == 0) await GetOrCreateUser();
            Rays = await http.GetJsonAsync<List<Ray>>($"/api/Prism/Remove/{CurrentUser.Id}/{RayId}");
            UpdatedRays?.Invoke();
        }

    }
}
