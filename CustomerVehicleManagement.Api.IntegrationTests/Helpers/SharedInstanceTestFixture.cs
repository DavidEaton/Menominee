using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Helpers
{
    public abstract class SharedInstanceTestFixture : IClassFixture<TestApplicationFactory<Startup, TestStartup>>
    {
        protected CustomWebApplicationFactory<TestStartup> Factory { get; }

        public SharedInstanceTestFixture(TestApplicationFactory<Startup, TestStartup> factory)
        {
            Factory = factory;
        }

        // Add your other helper methods here
    }
}