using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Services
{
    public class RepairOrderServiceToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderId { get; set; } = 0;
        public int SequenceNumber { get; set; } = 0;
        public string ServiceName { get; set; } = string.Empty;
        public string SaleCode { get; set; } = string.Empty;
        public bool IsCounterSale { get; set; } = false;
        public bool IsDeclined { get; set; } = false;
        public double PartsTotal { get; set; } = 0.0;
        public double LaborTotal { get; set; } = 0.0;
        public double TaxTotal { get; set; } = 0.0;
        public double ShopSuppliesTotal { get; set; } = 0.0;
        public double Total { get; set; } = 0.0;

        public IList<RepairOrderItemToWrite> Items { get; set; } = new List<RepairOrderItemToWrite>();
        public IList<RepairOrderTechToWrite> Techs { get; set; } = new List<RepairOrderTechToWrite>();
        public IList<RepairOrderServiceTaxToWrite> Taxes { get; set; } = new List<RepairOrderServiceTaxToWrite>();
    }
}
