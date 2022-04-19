using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders
{
    public class RepairOrderToWrite
    {
        public long RepairOrderNumber { get; set; } = 0;
        public long InvoiceNumber { get; set; } = 0;
        public string CustomerName { get; set; } = string.Empty;
        public string Vehicle { get; set; } = string.Empty;
        public double PartsTotal { get; set; } = 0.0;
        public double LaborTotal { get; set; } = 0.0;
        public double DiscountTotal { get; set; } = 0.0;
        public double TaxTotal { get; set; } = 0.0;
        public double HazMatTotal { get; set; } = 0.0;
        public double ShopSuppliesTotal { get; set; } = 0.0;
        public double Total { get; set; } = 0.0;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateInvoiced { get; set; }

        public List<RepairOrderServiceToWrite> Services { get; set; } = new List<RepairOrderServiceToWrite>();
        public List<RepairOrderTaxToWrite> Taxes { get; set; } = new List<RepairOrderTaxToWrite>();
        public List<RepairOrderPaymentToWrite> Payments { get; set; } = new List<RepairOrderPaymentToWrite>();

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrder.cs
        public void Recalculate()
        {
            PartsTotal = 0.0;
            LaborTotal = 0.0;
            DiscountTotal = 0.0;
            TaxTotal = 0.0;
            ShopSuppliesTotal = 0.0;

            if (Services?.Count > 0)
            {
                foreach (var service in Services)
                {
                    PartsTotal += service.PartsTotal;
                    LaborTotal += service.LaborTotal;
                    DiscountTotal += service.DiscountTotal;
                    ShopSuppliesTotal += service.ShopSuppliesTotal;
                }
            }

            if (Taxes?.Count > 0)
            {
                foreach (var tax in Taxes)
                {
                    TaxTotal += (tax.PartTax + tax.LaborTax);
                }
            }

            Total = PartsTotal + LaborTotal - DiscountTotal + ShopSuppliesTotal;
        }
    }
}
