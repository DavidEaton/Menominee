using MenomineePlayWASM.Shared.Dtos.Payables.Invoices;
using Microsoft.AspNetCore.Components;

namespace MenomineePlayWASM.Client.Components.Payables
{
    public partial class VendorInvoiceForm
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
