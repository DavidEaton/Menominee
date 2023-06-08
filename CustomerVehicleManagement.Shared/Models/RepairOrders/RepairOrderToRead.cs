using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.Customers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Statuses;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders
{
    public class RepairOrderToRead
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
        public DateTime DateInvoiced =>
            Statuses.OrderByDescending(repairOrderStatus => repairOrderStatus.Status)
                    .Select(repairOrderStatus => repairOrderStatus.Date)
                    .FirstOrDefault();
        public Status Status =>
            Statuses.OrderByDescending(repairOrderStatus => repairOrderStatus.Date)
                    .Select(repairOrderStatus => repairOrderStatus.Status)
                    .FirstOrDefault();

        public List<RepairOrderStatusToRead> Statuses { get; set; } = new List<RepairOrderStatusToRead>();
        public List<RepairOrderServiceToRead> Services { get; set; } = new List<RepairOrderServiceToRead>();
        public List<RepairOrderTaxToRead> Taxes { get; set; } = new List<RepairOrderTaxToRead>();
        public List<RepairOrderPaymentToRead> Payments { get; set; } = new List<RepairOrderPaymentToRead>();
    }
}
