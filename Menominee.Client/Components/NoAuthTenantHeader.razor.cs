using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Menominee.Common.Entities;
using System;
using System.Threading.Tasks;

namespace Menominee.Client.Components
{
    public partial class NoAuthTenantHeader : ComponentBase
    {
        public Tenant Tenant { get; set; }

        protected override Task OnInitializedAsync()
        {
            Tenant = new Tenant
            {
                CompanyName = "Janco Development",
                Name = "Janco Development",
                LogoUrl = "https://menominee.blob.core.windows.net/new-tenant/new-tenant"
            };
            return Task.CompletedTask;
        }

        protected static void ImageClick(MouseEventArgs e)
        {
            Console.WriteLine("Tenant image clicked! ");
        }
    }
}
