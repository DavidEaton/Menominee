using Menominee.Domain.Entities;

namespace Menominee.Shared.Models.SaleCodes
{
    public class SaleCodeHelper
    {
        public static SaleCodeToWrite ConvertReadToWriteDto(SaleCodeToRead saleCode)
        {
            return saleCode is null
                ? new()
                : new()
                {
                    Code = saleCode.Code,
                    Name = saleCode.Name,
                    DesiredMargin = saleCode.DesiredMargin,
                    LaborRate = saleCode.LaborRate,
                    ShopSupplies = new()
                    {
                        IncludeLabor = saleCode.ShopSupplies.IncludeLabor,
                        IncludeParts = saleCode.ShopSupplies.IncludeParts,
                        MaximumCharge = saleCode.ShopSupplies.MaximumCharge,
                        MinimumCharge = saleCode.ShopSupplies.MinimumCharge,
                        MinimumJobAmount = saleCode.ShopSupplies.MinimumJobAmount,
                        Percentage = saleCode.ShopSupplies.Percentage
                    }
                };
        }

        public static SaleCodeToRead ConvertToReadDto(SaleCode saleCode)
        {
            return saleCode is null
                ? new()
                : new()
                {
                    Id = saleCode.Id,
                    Code = saleCode.Code,
                    Name = saleCode.Name,
                    DesiredMargin = saleCode.DesiredMargin,
                    LaborRate = saleCode.LaborRate,
                    ShopSupplies =
                        saleCode.ShopSupplies is not null
                        ?
                        new()
                        {
                            IncludeLabor = saleCode.ShopSupplies.IncludeLabor,
                            IncludeParts = saleCode.ShopSupplies.IncludeParts,
                            MaximumCharge = saleCode.ShopSupplies.MaximumCharge,
                            MinimumCharge = saleCode.ShopSupplies.MinimumCharge,
                            MinimumJobAmount = saleCode.ShopSupplies.MinimumJobAmount,
                            Percentage = saleCode.ShopSupplies.Percentage
                        }
                        : null
                };
        }

        public static SaleCodeToReadInList ConvertToReadInListDto(SaleCode saleCode)
        {
            return saleCode is null
                ? new()
                : new()
                {
                    Id = saleCode.Id,
                    Code = saleCode.Code,
                    Name = saleCode.Name,
                    LaborRate = saleCode.LaborRate,
                    DesiredMargin = saleCode.DesiredMargin
                };
        }

        public static SaleCodeShopSuppliesToReadInList ConvertShopSuppliesToReadInListDto(SaleCode saleCode)
        {
            return saleCode is null
                ? new()
                : new()
                {
                    Id = saleCode.Id,
                    Code = saleCode.Code,
                    Name = saleCode.Name,
                    Percentage = saleCode.ShopSupplies.Percentage,
                    MinimumJobAmount = saleCode.ShopSupplies.MinimumJobAmount,
                    MinimumCharge = saleCode.ShopSupplies.MinimumCharge,
                    MaximumCharge = saleCode.ShopSupplies.MaximumCharge,
                    IncludeParts = saleCode.ShopSupplies.IncludeParts,
                    IncludeLabor = saleCode.ShopSupplies.IncludeLabor
                };
        }

        public static SaleCodeShopSuppliesToRead ConvertShopSuppliesToReadDto(SaleCode saleCode)
        {
            return saleCode is null
                ? new()
                : new()
                {
                    Id = saleCode.Id,
                    Percentage = saleCode.ShopSupplies.Percentage,
                    MinimumJobAmount = saleCode.ShopSupplies.MinimumJobAmount,
                    MinimumCharge = saleCode.ShopSupplies.MinimumCharge,
                    MaximumCharge = saleCode.ShopSupplies.MaximumCharge,
                    IncludeParts = saleCode.ShopSupplies.IncludeParts,
                    IncludeLabor = saleCode.ShopSupplies.IncludeLabor
                };
        }

        public static SaleCodeToRead ConvertReadInListToReadDto(SaleCodeToReadInList saleCode)
        {
            return saleCode is null
                ? new()
                : new()
                {
                    Id = saleCode.Id,
                    Code = saleCode.Code,
                    Name = saleCode.Name,
                    DesiredMargin = saleCode.DesiredMargin,
                    LaborRate = saleCode.LaborRate
                };
        }
    }
}
