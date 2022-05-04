using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ShopSuppliesPercentage = saleCode.ShopSuppliesPercentage,
                ShopSuppliesMinimumJobAmount = saleCode.ShopSuppliesMinimumJobAmount,
                ShopSuppliesMinimumCharge = saleCode.ShopSuppliesMinimumCharge,
                ShopSuppliesMaximumCharge = saleCode.ShopSuppliesMaximumCharge,
                ShopSuppliesIncludeParts = saleCode.ShopSuppliesIncludeParts,
                ShopSuppliesIncludeLabor = saleCode.ShopSuppliesIncludeLabor
            };
        }

        public static SaleCodeToWrite Transform(SaleCodeToRead saleCode)
        {
            if (saleCode is null)
                return null;

            return new SaleCodeToWrite
            {
                Code = saleCode.Code,
                Name = saleCode.Name,
                DesiredMargin = saleCode.DesiredMargin,
                LaborRate = saleCode.LaborRate,
                ShopSuppliesPercentage = saleCode.ShopSuppliesPercentage,
                ShopSuppliesMinimumJobAmount = saleCode.ShopSuppliesMinimumJobAmount,
                ShopSuppliesMinimumCharge = saleCode.ShopSuppliesMinimumCharge,
                ShopSuppliesMaximumCharge = saleCode.ShopSuppliesMaximumCharge,
                ShopSuppliesIncludeParts = saleCode.ShopSuppliesIncludeParts,
                ShopSuppliesIncludeLabor = saleCode.ShopSuppliesIncludeLabor
            };
        }

        public static SaleCodeToRead Transform(SaleCode saleCode)
        {
            if (saleCode is null)
                return null;

            return new SaleCodeToRead
            {
                Id = saleCode.Id,
                Code = saleCode.Code,
                Name = saleCode.Name,
                DesiredMargin = saleCode.DesiredMargin,
                LaborRate = saleCode.LaborRate,
                ShopSuppliesPercentage = saleCode.ShopSuppliesPercentage,
                ShopSuppliesMinimumJobAmount = saleCode.ShopSuppliesMinimumJobAmount,
                ShopSuppliesMinimumCharge = saleCode.ShopSuppliesMinimumCharge,
                ShopSuppliesMaximumCharge = saleCode.ShopSuppliesMaximumCharge,
                ShopSuppliesIncludeParts = saleCode.ShopSuppliesIncludeParts,
                ShopSuppliesIncludeLabor = saleCode.ShopSuppliesIncludeLabor
            };
        }
    }
}
