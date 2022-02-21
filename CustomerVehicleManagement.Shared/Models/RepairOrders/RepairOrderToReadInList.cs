using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using System;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders
{
    public class RepairOrderToReadInList
    {
        public long Id { get; set; }
        public long RepairOrderNumber { get; set; }
        public long InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string Vehicle { get; set; }
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

        public static RepairOrderToReadInList ConvertToDto(RepairOrder repairOrder)
        {
            if (repairOrder != null)
            {
                return new RepairOrderToReadInList
                {
                    Id = repairOrder.Id,
                    RepairOrderNumber = repairOrder.RepairOrderNumber,
                    InvoiceNumber = repairOrder.InvoiceNumber,
                    CustomerName = repairOrder.CustomerName,
                    Vehicle = repairOrder.Vehicle,
                    PartsTotal = repairOrder.PartsTotal,
                    LaborTotal = repairOrder.LaborTotal,
                    DiscountTotal = repairOrder.DiscountTotal,
                    TaxTotal = repairOrder.TaxTotal,
                    HazMatTotal = repairOrder.HazMatTotal,
                    ShopSuppliesTotal = repairOrder.ShopSuppliesTotal,
                    Total = repairOrder.Total,
                    DateCreated = repairOrder.DateCreated,
                    DateModified = repairOrder.DateModified,
                    DateInvoiced = repairOrder.DateInvoiced
                };
            }

            return null;
        }
    }
}
