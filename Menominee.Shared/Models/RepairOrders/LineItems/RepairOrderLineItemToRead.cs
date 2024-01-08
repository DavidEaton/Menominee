using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;
using Menominee.Shared.Models.RepairOrders.LineItems.Item;
using Menominee.Shared.Models.RepairOrders.Purchases;
using Menominee.Shared.Models.RepairOrders.SerialNumbers;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.RepairOrders.Warranties;
using System.Collections.Generic;

namespace Menominee.Shared.Models.RepairOrders.LineItems
{
    public class RepairOrderLineItemToRead
    {
        public long Id { get; set; }
        public RepairOrderItemToRead Item { get; set; }
        public SaleType SaleType { get; set; }
        public bool IsDeclined { get; set; }
        public bool IsCounterSale { get; set; }
        public double QuantitySold { get; set; }
        public double SellingPrice { get; set; }
        public LaborAmountToRead LaborAmount { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public DiscountAmountToRead DiscountAmount { get; set; }
        public double Total { get; set; }

        public IReadOnlyList<RepairOrderSerialNumberToRead> SerialNumbers = new List<RepairOrderSerialNumberToRead>();
        public IReadOnlyList<RepairOrderWarrantyToRead> Warranties = new List<RepairOrderWarrantyToRead>();
        public IReadOnlyList<RepairOrderItemTaxToRead> Taxes = new List<RepairOrderItemTaxToRead>();
        public IReadOnlyList<RepairOrderPurchaseToRead> Purchases = new List<RepairOrderPurchaseToRead>();

    }
}
