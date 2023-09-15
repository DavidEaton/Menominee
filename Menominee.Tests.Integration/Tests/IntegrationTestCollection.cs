using Xunit;

namespace Menominee.Tests.Integration.Tests
{
    [CollectionDefinition("Integration")]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebApplicationFactory>
    {
        /* This class doesn't need any code; it's just the 
         * 'blueprint' for a collection of tests that 
         * share the same context. -David E */
    }
}
