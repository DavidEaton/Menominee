using Menominee.Client.Services;
using Menominee.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Menominee.Client.Components
{
    public partial class TenantHeader : ComponentBase
    {
        private string errorUrl = string.Empty;

        [Inject]
        public IConfiguration Configuration { get; set; }

        [Inject]
        public ITenantDataService TenantDataService { get; set; }

        [Inject]
        public ILogger<TenantHeader> Logger { get; set; }
        public Tenant Tenant { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = await TenantDataService.GetAsync();

            Tenant = result.IsSuccess
                ? result.Value
                : new Tenant { Name = "Error", LogoUrl = errorUrl };

            Logger.LogInformation("TenantHeader.OnInitializedAsync()");
            Tenant.LogoUrl = BuildTenantLogoPath(Tenant);
            errorUrl = $"{Configuration.GetValue<string>("Tenant:LogoImage:BaseUrl")}" +
                $"{Configuration.GetValue<string>("Tenant:LogoImage:ErrorImage")}";
        }

        private string BuildTenantLogoPath(Tenant tenant)
        {
            var basePath = Configuration.GetValue<string>("Tenant:LogoImage:BaseUrl");
            var path = $"{basePath}{tenant.Name}/{tenant.Name}";
            return path;
        }

        protected static void ImageClick(MouseEventArgs e)
        {
            // Handle click
        }
    }
}
