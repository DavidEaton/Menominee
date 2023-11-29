using Menominee.Common.Enums;
using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using System.Collections.Generic;

namespace Menominee.Api.Features.Inventory.InventoryItems.Package
{
    // TODO: clean this up
    public static class ValidatorHelper
    {
        public static ProductCode CreateProductCode(
            ManufacturerToRead manufacturerFromCaller,
            ProductCodeToRead productCodeFromCaller)
        {
            var manufacturer = CreateManufacturer(manufacturerFromCaller);
            var saleCode = CreateSaleCode(productCodeFromCaller);
            List<string> manufacturerCodes = new()
            {
                "11"
            };

            return ProductCode.Create(manufacturer, "A1", "A One", manufacturerCodes, saleCode)
                .Value;
        }

        public static SaleCode CreateSaleCode(ProductCodeToRead productCode)
        {
            var name = productCode.SaleCode.Name;
            var code = productCode.SaleCode.Code;
            var laborRate = productCode.SaleCode.LaborRate;
            var desiredMargin = productCode.SaleCode.DesiredMargin;
            var shopSupplies = SaleCodeShopSupplies.Create(
                productCode.SaleCode.ShopSupplies.Percentage,
                productCode.SaleCode.ShopSupplies.MinimumJobAmount,
                productCode.SaleCode.ShopSupplies.MinimumCharge,
                productCode.SaleCode.ShopSupplies.MaximumCharge,
                productCode.SaleCode.ShopSupplies.IncludeParts,
                productCode.SaleCode.ShopSupplies.IncludeLabor)
                .Value;

            return SaleCode.Create(name, code, laborRate, desiredMargin, shopSupplies, new List<string>()).Value;
        }

        public static Manufacturer CreateManufacturer(ManufacturerToRead manufacturer)
        {
            return Manufacturer.Create(
                manufacturer.Id,
                manufacturer.Name,
                manufacturer.Prefix,
                new List<string>(),
                new List<long>())
                .Value;
        }

        public static InventoryItemPart CreateInventoryItemPart()
        {
            return InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount,
                InstallablePart.MaximumMoneyAmount,
                InstallablePart.MaximumMoneyAmount,
                InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(
                    ItemLaborType.Flat,
                    LaborAmount.MinimumAmount,
                    SkillLevel.A)
                    .Value,
                fractional: false)
                .Value;
        }
    }
}
