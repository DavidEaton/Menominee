using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.SaleCodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.RepairOrders.LineItems.Item
{
    public class ItemHelper
    {
        internal static RepairOrderItemToWrite ConvertToWriteDto(RepairOrderItem item) =>
            item == null
                ? new RepairOrderItemToWrite()
                : new RepairOrderItemToWrite
                {
                    Description = item.Description,
                    Id = item.Id,
                    Labor = item.Labor?.ToWriteDto(),
                    Manufacturer = ManufacturerHelper.ConvertToReadDto(item.Manufacturer),
                    Part = item.Part?.ToWriteDto(),
                    PartNumber = item.PartNumber,
                    PartType = item.PartType,
                    ProductCode = ProductCodeHelper.ConvertToReadDto(item.ProductCode),
                    SaleCode = SaleCodeHelper.ConvertToReadDto(item.SaleCode)
                };

        internal static RepairOrderItem ConvertWriteDtoToEntity(
            RepairOrderItemToWrite item,
            IReadOnlyList<SaleCode> saleCodes,
            IReadOnlyList<ProductCode> productCodes,
            IReadOnlyList<Manufacturer> manufacturers)
        {
            if (item is null)
                return null;

            var manufacturerId = item?.Manufacturer?.Id;
            var manufacturer = manufacturers.FirstOrDefault(m => m.Id == manufacturerId);
            if (manufacturer is null)
                throw new InvalidOperationException($"Manufacturer with ID {manufacturerId} not found.");

            var saleCodeId = item?.SaleCode?.Id;
            var saleCode = saleCodes.FirstOrDefault(s => s.Id == saleCodeId);
            if (saleCode is null)
                throw new InvalidOperationException($"SaleCode with ID {saleCodeId} not found.");

            var productCodeId = item?.ProductCode?.Id;
            var productCode = productCodes.FirstOrDefault(p => p.Id == productCodeId);
            if (productCode is null)
                throw new InvalidOperationException($"ProductCode with ID {productCodeId} not found.");

            return RepairOrderItem.Create(
                manufacturer,
                item.PartNumber,
                item.Description,
                saleCode,
                productCode,
                item.PartType,
                item?.Part.ToEntity() ?? null,
                item?.Labor.ToEntity() ?? null)
                .Value;
        }


        internal static RepairOrderItem ConvertWriteDtoToEntity(
            RepairOrderItemToWrite item,
            SaleCode saleCode,
            ProductCode productCode,
            Manufacturer manufacturer,
            IReadOnlyList<RepairOrderItemPart> itemParts)
        {
            return
                item is not null
                ? RepairOrderItem.Create(
                    manufacturer,
                    item.PartNumber,
                    item.Description,
                    saleCode,
                    productCode,
                    item.PartType,
                    item?.Part.ToEntity() ?? null,
                    item?.Labor.ToEntity() ?? null)
                    .Value
                : null;
        }
    }
}
