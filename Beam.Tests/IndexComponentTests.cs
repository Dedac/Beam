using Xunit;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Beam.Client.Pages;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Authorization;

namespace Beam.Tests
{
    public class IndexComponentTests : TestContext
    {
        // [Fact]
        // public void IndexPageRenders()
        // {
        //     Services.AddSingleton<IAuthorizationPolicyProvider, MockAuthorizationPolicyProvider>();
        //     Services.AddSingleton<IAuthorizationService, MockAuthorizationService>();
        //     Services.AddSingleton<AuthenticationStateProvider>(new TestAuthenticationStateProvider(null));

        //     var wrapper = RenderComponent<CascadingAuthenticationState>(parameters => parameters.AddChildContent<Index>());

        //     // Arrange
        //     var cut = wrapper.FindComponent<Index>();
            
        //     // Assert that content of the paragraph shows counter at zero
        //     Assert.Contains("Select a Frequency, or create a new one", cut.Markup);
        //     cut.MarkupMatches(@"<h2 diff:ignore></h2>
        //                         <p diff:ignore></p>");
        // }
    }
}