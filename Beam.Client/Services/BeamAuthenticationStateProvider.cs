using System.Security.Claims;
using System.Threading.Tasks;
using Beam.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace Beam.Client.Services
{
    public class BeamAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly AuthenticationState AnonymousState =
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        private readonly BeamApiService _apiService;
        private readonly DataService _dataService;
        private AuthenticationState? _currentState;

        public BeamAuthenticationStateProvider(BeamApiService apiService, DataService dataService)
        {
            _apiService = apiService;
            _dataService = dataService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_currentState != null) return _currentState;

            var user = await _apiService.CurrentUser();
            _currentState = BuildState(user);

            return _currentState;
        }

        public async Task<AuthResult> Register(RegisterRequest request)
        {
            return Apply(await _apiService.Register(request));
        }

        public async Task<AuthResult> Login(LoginRequest request)
        {
            return Apply(await _apiService.Login(request));
        }

        public async Task<AuthResult> ChangePassword(ChangePasswordRequest request)
        {
            return Apply(await _apiService.ChangePassword(request));
        }

        public async Task Logout()
        {
            await _apiService.Logout();
            SetUser(null);
        }

        private AuthResult Apply(AuthResult result)
        {
            if (result.Succeeded) SetUser(result.User);

            return result;
        }

        private void SetUser(User? user)
        {
            _currentState = BuildState(user);
            NotifyAuthenticationStateChanged(Task.FromResult(_currentState));
        }

        private AuthenticationState BuildState(User? user)
        {
            _dataService.SetCurrentUser(user);

            if (user == null || string.IsNullOrWhiteSpace(user.Name)) return AnonymousState;

            var identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name)
                },
                authenticationType: "Beam");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
