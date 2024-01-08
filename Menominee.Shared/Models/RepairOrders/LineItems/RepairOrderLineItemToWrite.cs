using Menominee.Domain.Enums;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;
using Menominee.Shared.Models.RepairOrders.LineItems;
using Menominee.Shared.Models.RepairOrders.LineItems.Item;
using Menominee.Shared.Models.RepairOrders.Purchases;
using Menominee.Shared.Models.RepairOrders.SerialNumbers;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.RepairOrders.Warranties;
using System.Collections.Generic;

namespace Menominee.Shared.Models.RepairOrders.Items
{
    public class RepairOrderLineItemToWrite
    {
        public long Id { get; set; }
        public RepairOrderItemToWrite Item { get; set; } = new();
        public SaleType SaleType { get; set; } = SaleType.Regular;
        public bool IsDeclined { get; set; } = false;
        public bool IsCounterSale { get; set; } = false;
        public double QuantitySold { get; set; } = 0.0;
        public double SellingPrice { get; set; } = 0.0;
        public LaborAmountToRead LaborAmount { get; set; } = new();
        public DiscountAmountToRead DiscountAmount { get; set; } = new();
        public double Cost { get; set; } = 0.0;
        public double Core { get; set; } = 0.0;
        public double Total { get; set; } = 0.0;

        public List<RepairOrderSerialNumberToWrite> SerialNumbers { get; set; } = new();
        public List<RepairOrderWarrantyToWrite> Warranties { get; set; } = new();
        public List<RepairOrderItemTaxToWrite> Taxes { get; set; } = new();
        public List<RepairOrderPurchaseToWrite> Purchases { get; set; } = new();

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        public void Recalculate()
        {
            Total = (SellingPrice + LaborAmount.Amount - DiscountAmount.Amount) * QuantitySold;
        }
    }
}
