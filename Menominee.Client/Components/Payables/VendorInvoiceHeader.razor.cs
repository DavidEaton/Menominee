using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Client.Services.Payables.Vendors;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceHeader : ComponentBase
    {
        [Inject]
        public IVendorDataService VendorDataService { get; set; }

        [Parameter]
        public VendorInvoiceToWrite Invoice { get; set; }

        public IReadOnlyList<VendorToReadInList> Vendors;

        //protected override async Task OnInitializedAsync()
        protected override async Task OnParametersSetAsync()
        {
            Vendors = (await VendorDataService.GetAllVendors()).ToList();
        }
    }
}
