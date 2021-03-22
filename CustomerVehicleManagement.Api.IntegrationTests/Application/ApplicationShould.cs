using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CustomerVehicleManagement.Api.IntegrationTests.Application
{
    public class ApplicationShould
    {
        public TestServer Server { get; }
        public HttpClient Client { get; }

        public ApplicationShould()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(@"C:\Users\David\source\repos\Menominee\CustomerVehicleManagement.Api")
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            Server = new TestServer(builder);

            Client = Server.CreateClient();
        }

        [Fact]
        public async Task SomethingAsync()
        {
            //var response = await Client.GetAsync(":54382/persons/list");
            var response = await Client.GetAsync("/persons/list");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
        }

        [Fact]
        public async void TestVisitRoot()
        {
            var response = await Client.GetAsync("/");
            response.EnsureSuccessStatusCode();
        }
    }
}
