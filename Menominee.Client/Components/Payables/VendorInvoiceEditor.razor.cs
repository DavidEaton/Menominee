using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceEditor
    {
        [Parameter]
        public VendorInvoiceToWrite Invoice { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Invoice";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }
    }
}
