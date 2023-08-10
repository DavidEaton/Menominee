namespace Menominee.Shared.Models.RepairOrders.Services
{
    public class RepairOrderCalculator
    {
        public static RepairOrderToWrite RecalculateRepairOrder(RepairOrderToWrite repairOrder)
        {
            repairOrder.PartsTotal = 0.0;
            repairOrder.LaborTotal = 0.0;
            repairOrder.DiscountTotal = 0.0;
            repairOrder.TaxTotal = 0.0;
            repairOrder.ShopSuppliesTotal = 0.0;

            if (repairOrder.Services?.Count > 0)
            {
                foreach (var service in repairOrder.Services)
                {
                    repairOrder.PartsTotal += service.PartsTotal;
                    repairOrder.LaborTotal += service.LaborTotal;
                    repairOrder.DiscountTotal += service.DiscountTotal;
                    repairOrder.ShopSuppliesTotal += service.ShopSuppliesTotal;
                }
            }

            if (repairOrder.Taxes?.Count > 0)
            {
                foreach (var tax in repairOrder.Taxes)
                {
                    repairOrder.TaxTotal += (tax.PartTax.Amount + tax.LaborTax.Amount);
                }
            }

            repairOrder.Total = repairOrder.PartsTotal + repairOrder.LaborTotal - repairOrder.DiscountTotal + repairOrder.ShopSuppliesTotal;

            return repairOrder;
        }
    }
}
