using Beam.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Beam.Client.Services
{
    public class DataService : IDataService
    {
        public IReadOnlyList<Frequency> Frequencies { get; private set; }
        private IReadOnlyList<Ray> _rays = new List<Ray>();
        public IReadOnlyList<Ray> Rays
        {
            get => _rays;
            private set => _rays = value.OrderByDescending(r => r.RayId).ToList(); 
        } 
        
        public User CurrentUser { get; set; }

        private int? selectedFrequency;
        private readonly IBeamApiService apiService;
        private readonly NavigationManager navigationManager;

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

        public DataService(IBeamApiService apiService, NavigationManager navMan)
        {
            this.apiService = apiService;
            navigationManager = navMan;
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
                navigationManager.NavigateTo("/login");
                ray.UserId = CurrentUser.Id;
            }

            Rays = await apiService.AddRay(ray);
            UpdatedRays?.Invoke();
        }

        public async Task PrismRay(int RayId)
        {
            if (CurrentUser.Id == 0)
                navigationManager.NavigateTo("/login");
            Rays = await apiService.PrismRay(new Prism() { RayId = RayId, UserId = CurrentUser.Id });
            UpdatedRays?.Invoke();
        }

        public async Task UnPrismRay(int RayId)
        {
            if (CurrentUser.Id == 0)
                navigationManager.NavigateTo("/login");
            Rays = await apiService.UnPrismRay(RayId, CurrentUser.Id);
            UpdatedRays?.Invoke();
        }
    }
}
