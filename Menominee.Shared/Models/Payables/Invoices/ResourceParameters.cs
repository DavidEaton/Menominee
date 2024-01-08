using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.Payables.Invoices
{
    public class ResourceParameters
    {
        public long? VendorId { get; set; }
        public VendorInvoiceStatus? Status { get; set; }
    }
}
