using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders
{
    public class RepairOrderToRead
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

        public IReadOnlyList<RepairOrderServiceToRead> Services { get; set; } = new List<RepairOrderServiceToRead>();
        public IReadOnlyList<RepairOrderTaxToRead> Taxes { get; set; } = new List<RepairOrderTaxToRead>();
        public IReadOnlyList<RepairOrderPaymentToRead> Payments { get; set; } = new List<RepairOrderPaymentToRead>();

        public static RepairOrderToRead ConvertToDto(RepairOrder repairOrder)
        {
            if (repairOrder != null)
            {
                return new RepairOrderToRead()
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
                    DateInvoiced = repairOrder.DateInvoiced,
                    Services = RepairOrderServiceToRead.ConvertToDto(repairOrder.Services),
                    Taxes = RepairOrderTaxToRead.ConvertToDto(repairOrder.Taxes),
                    Payments = RepairOrderPaymentToRead.ConvertToDto(repairOrder.Payments)
                };
            }

            return null;
        }
    }
}
