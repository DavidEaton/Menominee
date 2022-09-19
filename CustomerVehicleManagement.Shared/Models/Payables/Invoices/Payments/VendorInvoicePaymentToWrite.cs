using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments
{
    public class VendorInvoicePaymentToWrite
    {
        public long Id { get; set; }
        public long PaymentMethodId { get; set; }
        public double Amount { get; set; } = 0.0;
        public TrackingState TrackingState { get; set; } = TrackingState.Unchanged;
    }
}
