using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Techs;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Services
{
    public class RepairOrderServiceToRead
    {
        public long Id { get; set; }
        public long RepairOrderId { get; set; }
        public string ServiceName { get; set; }
        public string SaleCode { get; set; }
        public bool IsCounterSale { get; set; }
        public bool IsDeclined { get; set; }
        public double PartsTotal { get; set; }
        public double LaborTotal { get; set; }
        public double DiscountTotal { get; set; }
        public double TaxTotal { get; set; }
        public double ShopSuppliesTotal { get; set; }
        public double Total { get; set; }

        public IList<RepairOrderItemToRead> Items { get; set; } = new List<RepairOrderItemToRead>();
        public IList<RepairOrderTechToRead> Techs { get; set; } = new List<RepairOrderTechToRead>();
        public IList<RepairOrderServiceTaxToRead> Taxes { get; set; } = new List<RepairOrderServiceTaxToRead>();
    }
}
