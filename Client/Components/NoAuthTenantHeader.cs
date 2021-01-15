using Microsoft.AspNetCore.Components;
using SharedKernel;
using System.Threading.Tasks;

namespace Client.Components
{
    public partial class NoAuthTenantHeader : ComponentBase
    {
        public Tenant Tenant { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Tenant = new Tenant
            {
                CompanyName = "Janco Development",
                Name = "Janco Development",
                LogoUrl = "https://stocktracstorage.blob.core.windows.net/new-tenant/new-tenant"
            };

            
        }

    }
}
