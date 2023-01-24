using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceTotals
    {
        [CascadingParameter]
        public InvoiceTotals Totals { get; set; }

    }
}
