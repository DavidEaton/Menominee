using Menominee.Shared.Models.Payables.Vendors;
using System;

namespace Menominee.Shared.Models.RepairOrders.Purchases
{
    public class PurchaseListItem
    {
        public VendorToRead Vendor { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string VendorPartNumber
        {
            get =>
                !string.IsNullOrWhiteSpace(PurchaseType?.VendorPartNumber)
                ? PurchaseType?.VendorPartNumber
                : string.Empty;

            set => PurchaseType.VendorPartNumber = value;
        }

        public string VendorInvoiceNumber
        {
            get =>
                !string.IsNullOrWhiteSpace(PurchaseType?.VendorInvoiceNumber)
                ? PurchaseType?.VendorInvoiceNumber
                : string.Empty;

            set => PurchaseType.VendorInvoiceNumber = value;
        }
        public string PONumber
        {
            get =>
                !string.IsNullOrWhiteSpace(PurchaseType?.PONumber)
                ? PurchaseType?.PONumber
                : string.Empty;

            set => PurchaseType.PONumber = value;
        }

        public double Quantity { get; set; }
        public double FileCost { get; set; }
        public double VendorCost { get; set; }
        public double VendorCore { get; set; }
        public DateTime? PurchaseDate
        {
            get => PurchaseType?.PurchaseDate;
            set
            {
                if (PurchaseType is not null)
                    PurchaseType.PurchaseDate = value;
            }
        }
        public RepairOrderPurchaseToWrite PurchaseType { get; set; }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(PartNumber);

            //return !string.IsNullOrWhiteSpace(VendorName) && !string.IsNullOrWhiteSpace(PurchaseType?.VendorInvoiceNumber) && VendorCost > 0.0;
        }
    }
}
