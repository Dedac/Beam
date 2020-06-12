﻿using Beam.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beam.Client.Services
{
    public class DataService : IDataService
    {
        public IReadOnlyList<Frequency> Frequencies { get; private set; }
        public IReadOnlyList<Ray> Rays { get; private set; } = new List<Ray>();
        public User CurrentUser { get; set; }
        
        private int? selectedFrequency;
        private readonly IBeamApiService apiService;

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

        public DataService(IBeamApiService apiService)
        {
            if (CurrentUser == null) CurrentUser = new User() { Name = "Anon" + new Random().Next(0, 10) };
            this.apiService = apiService;
        }

        public event Action UdpatedFrequencies;
        public event Action UpdatedRays;

        public async Task GetFrequencies()
        {
            Frequencies = await apiService.FrequencyList(); 
            UdpatedFrequencies?.Invoke();
        }

        public async Task GetRays(int FrequencyId)
        {
            Rays = new List<Ray>();
            Rays = await apiService.RayList(FrequencyId); 
            UpdatedRays?.Invoke();
        }

        public Task<List<Ray>> GetUserRays(string Name)
        {
            return apiService.GetUserRays(Name ?? CurrentUser.Name);
        }

        public async Task AddFrequency(string Name)
        {
            Frequencies = await apiService.AddFrequency(new Frequency() { Name = Name });  
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

            Rays = await apiService.AddRay(ray); 
            UpdatedRays?.Invoke();
        }

        public async Task PrismRay(int RayId)
        {
            if (CurrentUser.Id == 0) await GetOrCreateUser();
            Rays = await apiService.PrismRay(new Prism() { RayId = RayId, UserId = CurrentUser.Id }); 
            UpdatedRays?.Invoke();
        }

        public async Task UnPrismRay(int RayId)
        {
            if (CurrentUser.Id == 0) await GetOrCreateUser();
            Rays = await apiService.UnPrismRay(RayId, CurrentUser.Id); 
            UpdatedRays?.Invoke();
        }

        public async Task<User> GetOrCreateUser(string newName = null)
        {
            var name = newName ?? CurrentUser.Name;
            CurrentUser = await apiService.GetUser(name);
            return CurrentUser;
        }
       
    }
}
