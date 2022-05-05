using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Shared.Models.SaleCodes
{
    public class SaleCodeHelper
    {
        public static SaleCode Transform(SaleCodeToWrite saleCode)
        {
            if (saleCode == null)
                return null;

            return new SaleCode()
            {
                Code = saleCode.Code,
                Name = saleCode.Name,
                DesiredMargin = saleCode.DesiredMargin,
                LaborRate = saleCode.LaborRate,

                ShopSupplies = new SaleCodeShopSupplies()
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

        public static SaleCodeToWrite Transform(SaleCodeToRead saleCode)
        {
            if (saleCode is null)
                return null;

            return new SaleCodeToWrite()
            {
                Code = saleCode.Code,
                Name = saleCode.Name,
                DesiredMargin = saleCode.DesiredMargin,
                LaborRate = saleCode.LaborRate,
                ShopSupplies = new SaleCodeShopSuppliesToWrite()
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

        public static SaleCodeToRead Transform(SaleCode saleCode)
        {
            if (saleCode is null)
                return null;

            return new SaleCodeToRead()
            {
                Id = saleCode.Id,
                Code = saleCode.Code,
                Name = saleCode.Name,
                DesiredMargin = saleCode.DesiredMargin,
                LaborRate = saleCode.LaborRate,
                ShopSupplies = new SaleCodeShopSuppliesToRead()
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

        public static SaleCodeToReadInList ConvertToDto(SaleCode sc)
        {
            if (sc != null)
            {
                return new SaleCodeToReadInList
                {
                    Id = sc.Id,
                    Code = sc.Code,
                    Name = sc.Name,
                    LaborRate = sc.LaborRate,
                    DesiredMargin = sc.DesiredMargin
                };
            }

            return null;
        }
    }
}
