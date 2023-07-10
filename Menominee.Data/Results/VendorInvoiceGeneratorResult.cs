using Menominee.Domain.Entities.Payables;

namespace Menominee.Data.Results
{
    public static class VendorInvoiceGeneratorResult
    {
        public static IReadOnlyList<VendorInvoice> VendorInvoices { get; set; } = new List<VendorInvoice>();

    }
}
