using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.RepairOrders.LineItems.Item;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.LineItems
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
