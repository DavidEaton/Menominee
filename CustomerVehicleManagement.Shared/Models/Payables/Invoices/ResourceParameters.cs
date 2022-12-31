using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices
{
    public class ResourceParameters
    {
        public long? VendorId { get; set; }
        public VendorInvoiceStatus? Status { get; set; }
    }
}
