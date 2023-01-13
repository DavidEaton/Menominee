using CustomerVehicleManagement.Domain.Entities.Payables;

namespace CustomerVehicleManagement.Data.Results
{
    public static class VendorInvoiceGeneratorResult
    {
        public static IReadOnlyList<VendorInvoice> VendorInvoices { get; set; } = new List<VendorInvoice>();

    }
}
