using Xunit;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Beam.Client.Pages;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Authorization;
using Beam.Tests.Auth;
using Microsoft.AspNetCore.Components;

namespace Beam.Tests
{
    public class IndexComponentTests : TestContext
    {
        [Fact]
        public void IndexPageRenders()
        {
            Services.AddAuthenticationServices(TestAuthenticationStateProvider.CreateAuthenticationState("TestUser"));
            var wrapper = RenderComponent<CascadingAuthenticationState>(parameters => parameters.AddChildContent<Index>());

            // Arrange
            var cut = wrapper.FindComponent<Index>();
            
            // Assert that content of the paragraph shows counter at zero
            Assert.Contains("Select a Frequency, or create a new one", cut.Markup);
            cut.MarkupMatches(@"<h2 diff:ignore></h2>
                                <p diff:ignore></p>");
        }

        [Fact]
        public void IndexShowsSimpleHeaderWhenUnAuthorized()
        {
            Services.AddAuthenticationServices(TestAuthenticationStateProvider.CreateUnauthenticationState(), AuthorizationResult.Failed());
            Services.AddScoped<NavigationManager, MockNavigationManager>();

            var wrapper = RenderComponent<CascadingAuthenticationState>(parameters => parameters.AddChildContent<Index>());

            // Arrange
            var cut = wrapper.FindComponent<Index>();
            
            // Assert that content of the paragraph shows counter at zero
            Assert.DoesNotContain("Select a Frequency, or create a new one", cut.Markup);
            cut.MarkupMatches(@"<h1 diff:ignore></h1>");
        }
    }
}