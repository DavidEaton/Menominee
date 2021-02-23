using Microsoft.AspNetCore.Mvc.Testing;

namespace CustomerVehicleManagement.Api.IntegrationTests
{
    public class CustomWebApplicationFactory<Startup> : WebApplicationFactory<Startup>
        where Startup : class
    {
    }
}
