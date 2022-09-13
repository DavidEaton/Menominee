using CustomerVehicleManagement.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerVehicleManagement.Api.Data
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policies.RequireAuthenticatedUser)]
    public class ApplicationController : ControllerBase
    {
    }
}
