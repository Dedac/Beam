using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

public class MockAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
       return Task.FromResult<AuthorizationPolicy>(null);
    }

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
    {
       return Task.FromResult<AuthorizationPolicy>(null);
    }

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        return Task.FromResult<AuthorizationPolicy>(null);
    }
}

public class MockAuthorizationService : IAuthorizationService
{
    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
    {
        return Task.FromResult(AuthorizationResult.Success());
    }

    public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
    {
        return Task.FromResult(AuthorizationResult.Success());
    }
}

 public class TestAuthenticationStateProvider : AuthenticationStateProvider
    {
        public TestAuthenticationStateProvider(Task<AuthenticationState> state)
        {
            this.CurrentAuthStateTask = state;
        }

        public TestAuthenticationStateProvider()
        {
        }

        public Task<AuthenticationState> CurrentAuthStateTask { get; set; }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return CurrentAuthStateTask;
        }

        public void TriggerAuthenticationStateChanged(Task<AuthenticationState> authState)
        {
            this.NotifyAuthenticationStateChanged(authState);
        }

        public static Task<AuthenticationState> CreateAuthenticationState(string username)
        {
            var principal = new ClaimsPrincipal();
            return Task.FromResult(new AuthenticationState(principal));
        }

        public static Task<AuthenticationState> CreateUnauthenticationState()
        {
            var principal = new ClaimsPrincipal();
            return Task.FromResult(new AuthenticationState(principal));
        }
    }