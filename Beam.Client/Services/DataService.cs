using Beam.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beam.Client.Services
{
    public class DataService
    {
        private readonly BeamApiService _apiService;
        public IReadOnlyList<Frequency> Frequencies { get; private set; } = new List<Frequency>();
        public IReadOnlyList<Ray> Rays { get; private set; } = new List<Ray>();

        public User? CurrentUser { get; private set; }
        public bool IsSignedIn => CurrentUser != null;
        public string CurrentUserName => CurrentUser?.Name ?? string.Empty;

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

        public DataService(BeamApiService apiService)
        {
            _apiService = apiService;
        }

        public event Action? UdpatedFrequencies;
        public event Action? UpdatedRays;

        public void SetCurrentUser(User? user)
        {
            CurrentUser = user;
        }

        public async Task GetFrequencies()
        {
            Frequencies = await _apiService.FrequencyList(); 
            UdpatedFrequencies?.Invoke();
        }

        public async Task GetRays(int FrequencyId)
        {
            Rays = new List<Ray>();
            Rays = await _apiService.RayList(FrequencyId); 
            UpdatedRays?.Invoke();
        }

        public async Task AddFrequency(string Name)
        {
            if (!IsSignedIn || string.IsNullOrWhiteSpace(Name)) return;

            var frequencies = await _apiService.AddFrequency(new Frequency() { Name = Name });

            if (frequencies == null) return;

            Frequencies = frequencies;
            UdpatedFrequencies?.Invoke();
        }

        public async Task CreateRay(string text)
        {
            if (!IsSignedIn) return;

            var ray = new Ray()
            {
                FrequencyId = selectedFrequency ?? 0,
                Text = text
            };

            Rays = await _apiService.AddRay(ray); 
            UpdatedRays?.Invoke();
        }

        public async Task PrismRay(int RayId)
        {
            if (!IsSignedIn) return;

            Rays = await _apiService.PrismRay(new Prism() { RayId = RayId });
            UpdatedRays?.Invoke();
        }

        public async Task UnPrismRay(int RayId)
        {
            if (!IsSignedIn) return;

            Rays = await _apiService.UnPrismRay(RayId);
            UpdatedRays?.Invoke();
        }

        public async Task<List<Ray>> GetUserRays(string name)
        {
            var userName = string.IsNullOrWhiteSpace(name) ? CurrentUserName : name;

            if (string.IsNullOrWhiteSpace(userName)) return new List<Ray>();

            return await _apiService.UserRays(userName);
        }

    }
}
