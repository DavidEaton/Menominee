using Menominee.Shared.Models.RepairOrders.Items;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.RepairOrders.Techs;
using Menominee.Shared.Models.SaleCodes;
using System.Collections.Generic;

namespace Menominee.Shared.Models.RepairOrders.Services
{
    public class RepairOrderServiceToWrite
    {
        public long Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public SaleCodeToRead SaleCode { get; set; }
        public bool IsCounterSale { get; set; } = false;
        public bool IsDeclined { get; set; } = false;
        public double PartsTotal { get; set; } = 0.0;
        public double LaborTotal { get; set; } = 0.0;
        public double DiscountTotal { get; set; } = 0.0;
        public double TaxTotal { get; set; } = 0.0;
        public double ShopSuppliesTotal { get; set; } = 0.0;
        public double Total { get; set; } = 0.0;

        public List<RepairOrderLineItemToWrite> LineItems { get; set; } = new();
        public List<RepairOrderServiceTechnicianToWrite> Techs { get; set; } = new();
        public List<RepairOrderServiceTaxToWrite> Taxes { get; set; } = new();

        // TODO: Move Recalculate() method out of this data contract (it should contain no behavior).
        public void Recalculate()
        {
            PartsTotal = 0.0;
            LaborTotal = 0.0;
            DiscountTotal = 0.0;
            TaxTotal = 0.0;
            ShopSuppliesTotal = 0.0;

            if (LineItems?.Count > 0)
            {
                foreach (var lineItem in LineItems)
                {
                    PartsTotal += lineItem.SellingPrice * lineItem.QuantitySold;
                    LaborTotal += lineItem.LaborAmount.Amount * lineItem.QuantitySold;
                    DiscountTotal += lineItem.DiscountAmount.Amount * lineItem.QuantitySold;
                }
            }

            if (Taxes?.Count > 0)
            {
                foreach (var tax in Taxes)
                {
                    TaxTotal += (tax.PartTax.Amount + tax.LaborTax.Amount);
                }
            }

            Total = PartsTotal + LaborTotal - DiscountTotal + ShopSuppliesTotal;
        }
    }
}
