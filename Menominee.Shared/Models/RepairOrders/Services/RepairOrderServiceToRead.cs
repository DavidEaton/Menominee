using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.RepairOrders.LineItems;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.RepairOrders.Techs;
using Menominee.Shared.Models.SaleCodes;
using System.Collections.Generic;

namespace Menominee.Shared.Models.RepairOrders.Services
{
    public class RepairOrderServiceToRead
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public SaleCodeToRead SaleCode { get; set; }
        public bool IsCounterSale { get; set; }
        public bool IsDeclined { get; set; }
        public double PartsTotal { get; set; }
        public double LaborTotal { get; set; }
        public double DiscountTotal { get; set; }
        public double TaxTotal { get; set; }
        public double ShopSuppliesTotal { get; set; }
        public double Total { get; set; }

        public IList<RepairOrderLineItemToRead> Items { get; set; } = new List<RepairOrderLineItemToRead>();
        public IList<RepairOrderServiceTechnicianToRead> Techs { get; set; } = new List<RepairOrderServiceTechnicianToRead>();
        public IList<RepairOrderServiceTaxToRead> Taxes { get; set; } = new List<RepairOrderServiceTaxToRead>();
    }
}
