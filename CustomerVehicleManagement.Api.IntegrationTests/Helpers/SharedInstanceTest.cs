using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Helpers
{
    public abstract class SharedInstanceTest : IClassFixture<TestApplicationFactory<Startup, TestStartup>>
    {
        protected CustomWebApplicationFactory<TestStartup> Factory { get; }

        public SharedInstanceTest(TestApplicationFactory<Startup, TestStartup> factory)
        {
            Factory = factory;
        }

        // Add your other helper methods here
    }
}