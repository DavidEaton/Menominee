using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.Payables.Invoices.Items
{
    public class VendorInvoiceItemToWrite
    {
        public long Id { get; set; } = 0;
        public long VendorInvoiceId { get; set; } = 0;
        public VendorInvoiceItemType Type { get; set; } = VendorInvoiceItemType.Purchase;
        public string PartNumber { get; set; } = string.Empty;
        //public string MfrId { get; set; } = string.Empty;
        public ManufacturerToWrite Manufacturer { get; set; }
        //public long ManufacturerId { get; set; }
        public string Description { get; set; } = string.Empty;
        public double Quantity { get; set; } = 0.0;
        public double Cost { get; set; } = 0.0;
        public double Core { get; set; } = 0.0;
        public string PONumber { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime? TransactionDate { get; set; }
    }
}
