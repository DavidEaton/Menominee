using Menominee.Shared.Models.Customers;
using Menominee.Shared.Models.RepairOrders.Payments;
using Menominee.Shared.Models.RepairOrders.Services;
using Menominee.Shared.Models.RepairOrders.Statuses;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.Vehicles;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.RepairOrders
{
    public class RepairOrderToWrite
    {
        public long Id { get; set; }
        public long RepairOrderNumber { get; set; } = 0;
        public long InvoiceNumber { get; set; } = 0;
        public CustomerToWrite Customer { get; set; }
        public VehicleToWrite Vehicle { get; set; }
        public double PartsTotal { get; set; } = 0.0;
        public double LaborTotal { get; set; } = 0.0;
        public double DiscountTotal { get; set; } = 0.0;
        public double TaxTotal { get; set; } = 0.0;
        public double HazMatTotal { get; set; } = 0.0;
        public double ShopSuppliesTotal { get; set; } = 0.0;
        public double Total { get; set; } = 0.0;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime AccountingDate { get; set; }
        public List<RepairOrderStatusToWrite> Statuses { get; set; } = new List<RepairOrderStatusToWrite>();
        public List<RepairOrderServiceToWrite> Services { get; set; } = new List<RepairOrderServiceToWrite>();
        public List<RepairOrderTaxToWrite> Taxes { get; set; } = new List<RepairOrderTaxToWrite>();
        public List<RepairOrderPaymentToWrite> Payments { get; set; } = new List<RepairOrderPaymentToWrite>();
    }
}
