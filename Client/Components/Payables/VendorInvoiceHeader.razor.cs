using MenomineePlayWASM.Shared.Dtos.Payables.Invoices;
using MenomineePlayWASM.Shared.Dtos.Payables.Vendors;
using MenomineePlayWASM.Shared.Services.Payables.Vendors;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.Payables
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
