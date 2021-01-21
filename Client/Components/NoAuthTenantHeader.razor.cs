using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SharedKernel;
using System;
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

        protected static void ImageClick(MouseEventArgs e)
        {
            Console.WriteLine("Tenant image clicked! ");
        }
    }
}
