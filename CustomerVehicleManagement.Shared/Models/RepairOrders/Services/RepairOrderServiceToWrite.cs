using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Services
{
    public class RepairOrderServiceToWrite
    {
        public long RepairOrderId { get; set; } = 0;
        public string ServiceName { get; set; } = string.Empty;
        public string SaleCode { get; set; } = string.Empty;
        public bool IsCounterSale { get; set; } = false;
        public bool IsDeclined { get; set; } = false;
        public double PartsTotal { get; set; } = 0.0;
        public double LaborTotal { get; set; } = 0.0;
        public double DiscountTotal { get; set; } = 0.0;
        public double TaxTotal { get; set; } = 0.0;
        public double ShopSuppliesTotal { get; set; } = 0.0;
        public double Total { get; set; } = 0.0;

        public IList<RepairOrderItemToWrite> Items { get; set; } = new List<RepairOrderItemToWrite>();
        public IList<RepairOrderTechToWrite> Techs { get; set; } = new List<RepairOrderTechToWrite>();
        public IList<RepairOrderServiceTaxToWrite> Taxes { get; set; } = new List<RepairOrderServiceTaxToWrite>();

        public void Recalculate()
        {
            PartsTotal = 0.0;
            LaborTotal = 0.0;
            DiscountTotal = 0.0;
            TaxTotal = 0.0;
            ShopSuppliesTotal = 0.0;

            if (Items?.Count > 0)
            {
                foreach (var item in Items)
                {
                    PartsTotal += item.SellingPrice * item.QuantitySold;
                    LaborTotal += item.LaborEach * item.QuantitySold;
                    DiscountTotal += item.DiscountEach * item.QuantitySold;
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
