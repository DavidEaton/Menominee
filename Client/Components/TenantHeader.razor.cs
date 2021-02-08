﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Client.Services;
using System.Threading.Tasks;
using SharedKernel.Entities;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Client.Components
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
            Tenant = await TenantDataService.GetTenantAsync();
            Logger.LogInformation("TenantHeader.OnInitializedAsync()");
            Tenant.LogoUrl = BuildTenantLogoPath(Tenant);
            errorUrl = $"{Configuration.GetValue<string>("Tenant:LogoImage:BaseUrl")}" +
                $"{Configuration.GetValue<string>("Tenant:LogoImage:ErrorImage")}";
        }

        private string BuildTenantLogoPath(Tenant tenant)
        {
            string basePath = Configuration.GetValue<string>("Tenant:LogoImage:BaseUrl");
            string path = $"{basePath}{tenant.Name}/{tenant.Name}";
            return path;
        }

        protected static void ImageClick(MouseEventArgs e)
        {
            Console.WriteLine("Tenant image clicked! ");
        }
    }
}