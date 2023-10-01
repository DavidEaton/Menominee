using Xunit;

namespace Menominee.Tests.Integration.Tests
{
    [Collection("Integration")]
    public class GenericDataServiceShould : IntegrationTestBase
    {
        protected readonly string Route = "vehicles";
        public GenericDataServiceShould(IntegrationTestWebApplicationFactory factory) : base(factory)
        {
        }

        public override void SeedData()
        {
            throw new System.NotImplementedException();
        }
    }
}
