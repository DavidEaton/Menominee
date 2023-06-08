using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems.Labor;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.LineItems.Item;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.LineItems
{
    public class LineItemHelper
    {
        public static List<RepairOrderLineItemToWrite> CovertReadToWriteDtos(IList<RepairOrderLineItemToRead> lineItems)
        {
            return lineItems?.Select(
                item =>
                new RepairOrderLineItemToWrite
                {
                    Id = item.Id,
                    Core = item.Core,
                    Cost = item.Cost,
                    DiscountAmount = item.DiscountAmount,
                    LaborAmount = item.LaborAmount,
                    IsCounterSale = item.IsCounterSale,
                    IsDeclined = item.IsDeclined,
                    QuantitySold = item.QuantitySold,
                    SaleType = item.SaleType,
                    SellingPrice = item.SellingPrice,
                    Total = item.Total,
                    SerialNumbers = SerialNumberHelper.CovertReadToWriteDtos(item.SerialNumbers),
                    Warranties = WarrantyHelper.ConvertReadToWriteDtos(item.Warranties),
                    Taxes = ItemTaxHelper.ConvertReadToWriteDtos(item.Taxes),
                    Purchases = PurchaseHelper.ConvertReadToWriteDtos(item.Purchases)
                }).ToList()
                ?? new List<RepairOrderLineItemToWrite>();
        }

        public static IList<RepairOrderLineItemToRead> ConvertToReadDtos(IReadOnlyList<RepairOrderLineItem> items)
        {
            return items?.Select(
                lineItem =>
                new RepairOrderLineItemToRead()
                {
                    Id = lineItem.Id,
                    Core = lineItem.Core,
                    Cost = lineItem.Cost,
                    IsCounterSale = lineItem.IsCounterSale,
                    IsDeclined = lineItem.IsDeclined,
                    QuantitySold = lineItem.QuantitySold,
                    SaleType = lineItem.SaleType,
                    SellingPrice = lineItem.SellingPrice,
                    Total = lineItem.TotalAmount,
                    DiscountAmount = new DiscountAmountToRead()
                    {
                        Amount = lineItem.DiscountAmount.Amount,
                        DiscountType = lineItem.DiscountAmount.Type
                    },
                    LaborAmount = new LaborAmountToRead()
                    {
                        Amount = lineItem.LaborAmount.Amount,
                        PayType = lineItem.LaborAmount.PayType
                    },
                    Item = new RepairOrderItemToRead()
                    {
                        Manufacturer = ManufacturerHelper.ConvertToReadDto(lineItem.Item.Manufacturer),
                        Description = lineItem.Item.Description,
                        PartNumber = lineItem.Item.PartNumber,
                        PartType = lineItem.Item.PartType,
                        ProductCode = ProductCodeHelper.ConvertToReadDto(lineItem.Item.ProductCode),
                        SaleCode = SaleCodeHelper.ConvertToReadDto(lineItem.Item.SaleCode)
                    },
                    SerialNumbers = SerialNumberHelper.ConvertToReadDtos(lineItem.SerialNumbers),
                    Warranties = WarrantyHelper.ConvertToReadDtos(lineItem.Warranties),
                    Taxes = ItemTaxHelper.ConvertToReadDtos(lineItem.Taxes),
                    Purchases = PurchaseHelper.ConvertToReadDtos(lineItem.Purchases)
                }).ToList()
                ?? new List<RepairOrderLineItemToRead>();
        }
    }
}
