using Menominee.Domain.Entities;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Inventory.InventoryItems.Package
{
    // TODO: clean this up
    public static class ValidatorHelper
    {
        public static ProductCode CreateProductCode(
            ManufacturerToRead manufacturerFromCaller,
            ProductCodeToRead productCodeFromCaller)
        {
            Manufacturer manufacturer = CreateManufacturer(manufacturerFromCaller);
            SaleCode saleCode = CreateSaleCode(productCodeFromCaller);
            List<string> manufacturerCodes = new()
            {
                "11"
            };

            return ProductCode.Create(manufacturer, "A1", "A One", manufacturerCodes, saleCode)
                .Value;
        }

        public static SaleCode CreateSaleCode(ProductCodeToRead productCode)
        {
            string name = productCode.SaleCode.Name;
            string code = productCode.SaleCode.Code;
            double laborRate = productCode.SaleCode.LaborRate;
            double desiredMargin = productCode.SaleCode.DesiredMargin;
            SaleCodeShopSupplies shopSupplies = SaleCodeShopSupplies.Create(
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
