using Xunit;
using Bunit;

namespace Beam.Tests
{
    public class IndexComponentTests : TestContext
    {
        [Fact]
        public void IndexPageRenders()
        {
            // Arrange
            var cut = RenderComponent<Beam.Client.Pages.Index>();
            
            // Assert that content of the paragraph shows counter at zero
            Assert.Contains("Select a Frequency, or create a new one", cut.Markup);
            cut.MarkupMatches(@"<h2 diff:ignore></h2>
                                <p diff:ignore></p>");
        }
    }
}