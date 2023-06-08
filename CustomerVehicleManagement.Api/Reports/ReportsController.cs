using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;

namespace CustomerVehicleManagement.Api.Reports
{
    [IgnoreAntiforgeryToken]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ReportsControllerBase
    {
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
            : base(reportServiceConfiguration)
        {
        }
    }
}
