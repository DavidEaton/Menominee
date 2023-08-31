using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.Inventory.InventoryItems.Labor;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.RepairOrders.Items;
using Menominee.Shared.Models.RepairOrders.LineItems.Item;
using Menominee.Shared.Models.RepairOrders.Purchases;
using Menominee.Shared.Models.RepairOrders.SerialNumbers;
using Menominee.Shared.Models.RepairOrders.Taxes;
using Menominee.Shared.Models.RepairOrders.Warranties;
using Menominee.Shared.Models.SaleCodes;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.LineItems
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
                    Total = lineItem.AmountTotal,
                    DiscountAmount = new DiscountAmountToRead()
                    {
                        Amount = lineItem.DiscountAmount.Amount,
                        DiscountType = lineItem.DiscountAmount.Type
                    },
                    LaborAmount = new LaborAmountToRead()
                    {
                        Amount = lineItem.LaborAmount.Amount,
                        PayType = lineItem.LaborAmount.Type
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

        internal static List<RepairOrderLineItemToWrite> CovertToWriteDtos(IReadOnlyList<RepairOrderLineItem> lineItems)
        {
            return lineItems?.Select(
                lineItem =>
                new RepairOrderLineItemToWrite
                {
                    Id = lineItem.Id,
                    Item = ItemHelper.ConvertToWriteDto(lineItem.Item),
                    Core = lineItem.Core,
                    Cost = lineItem.Cost,
                    DiscountAmount = new DiscountAmountToRead() { DiscountType = lineItem.DiscountAmount.Type },
                    LaborAmount = new LaborAmountToRead() { Amount = lineItem.LaborAmount.Amount, PayType = lineItem.LaborAmount.Type },
                    IsCounterSale = lineItem.IsCounterSale,
                    IsDeclined = lineItem.IsDeclined,
                    QuantitySold = lineItem.QuantitySold,
                    SaleType = lineItem.SaleType,
                    SellingPrice = lineItem.SellingPrice,
                    Total = lineItem.AmountTotal,
                    SerialNumbers = SerialNumberHelper.CovertToWriteDtos(lineItem.SerialNumbers),
                    Warranties = WarrantyHelper.ConvertToWriteDtos(lineItem.Warranties),
                    Taxes = ItemTaxHelper.ConvertToWriteDtos(lineItem.Taxes),
                    Purchases = PurchaseHelper.ConvertToWriteDtos(lineItem.Purchases)
                }).ToList()
                ?? new List<RepairOrderLineItemToWrite>();
        }

        internal static List<RepairOrderLineItem> ConvertWriteDtosToEntities(
            List<RepairOrderLineItemToWrite> lineItems,
            IReadOnlyList<SaleCode> saleCodes,
            IReadOnlyList<ProductCode> productCodes,
            IReadOnlyList<Manufacturer> manufacturers)
        {
            return lineItems?.Select(
                item =>
                RepairOrderLineItem.Create(
                    ItemHelper.ConvertWriteDtoToEntity(item.Item, saleCodes, productCodes, manufacturers),
                    item.SaleType,
                    item.IsDeclined,
                    item.IsCounterSale,
                    item.QuantitySold,
                    item.SellingPrice,
                    item.LaborAmount is not null
                    ? LaborAmount.Create(item.LaborAmount.PayType, item.LaborAmount.Amount).Value
                    : LaborAmount.Create(Common.Enums.ItemLaborType.None, default(double)).Value,
                    item.Cost,
                    item.Core,
                    item.DiscountAmount is not null
                        ? DiscountAmount.Create(item.DiscountAmount.DiscountType, item.DiscountAmount.Amount).Value
                        : DiscountAmount.Create(Common.Enums.ItemDiscountType.None, default(double)).Value
                ).Value
            ).ToList() ?? new List<RepairOrderLineItem>();
        }
    }
}