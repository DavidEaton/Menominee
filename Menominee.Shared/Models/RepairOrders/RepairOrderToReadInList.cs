using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.Vehicles;
using System;

namespace Menominee.Shared.Models.RepairOrders
{
    public class RepairOrderToReadInList
    {
        public long Id { get; set; }
        public long RepairOrderNumber { get; set; }
        public long InvoiceNumber { get; set; }
        public CustomerToRead Customer { get; set; }
        public VehicleToRead Vehicle { get; set; }
        public double PartsTotal { get; set; }
        public double LaborTotal { get; set; }
        public double DiscountTotal { get; set; }
        public double TaxTotal { get; set; }
        public double HazMatTotal { get; set; }
        public double ShopSuppliesTotal { get; set; }
        public double Total { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateInvoiced { get; set; }
    }
}
