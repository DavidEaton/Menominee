﻿using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Shared.Models.SaleCodes
{
    public class SaleCodeHelper
    {
        public static SaleCode CreateSaleCode(SaleCodeToWrite saleCode)
        {
            if (saleCode == null)
                return null;

            return new()
            {
                Code = saleCode.Code.ToUpper(),
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

        public static SaleCodeToWrite CreateSaleCode(SaleCodeToRead saleCode)
        {
            if (saleCode is null)
                return null;

            return new()
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

        public static SaleCodeToRead CreateSaleCode(SaleCode saleCode)
        {
            if (saleCode is null)
                return null;

            return new()
            {
                Id = saleCode.Id,
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

        public static SaleCodeToReadInList CreateSaleCodeInList(SaleCode saleCode)
        {
            if (saleCode is null)
                return null;

            return new()
            {
                Id = saleCode.Id,
                Code = saleCode.Code,
                Name = saleCode.Name,
                LaborRate = saleCode.LaborRate,
                DesiredMargin = saleCode.DesiredMargin
            };
        }

        public static SaleCodeShopSuppliesToReadInList CreateSaleCodeShopSuppliesInList(SaleCode saleCode)
        {
            if (saleCode is null)
                return null;

            return new()
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

        public static void CopySaleCode(SaleCodeToWrite saleCodeToWrite, SaleCode saleCode)
        {
            saleCode.Code = saleCodeToWrite.Code;
            saleCode.Name = saleCodeToWrite.Name;
            saleCode.DesiredMargin = saleCodeToWrite.DesiredMargin;
            saleCode.LaborRate = saleCodeToWrite.LaborRate;
            saleCode.ShopSupplies.IncludeLabor = saleCodeToWrite.ShopSupplies.IncludeLabor;
            saleCode.ShopSupplies.IncludeParts = saleCodeToWrite.ShopSupplies.IncludeParts;
            saleCode.ShopSupplies.MaximumCharge = saleCodeToWrite.ShopSupplies.MaximumCharge;
            saleCode.ShopSupplies.MinimumCharge = saleCodeToWrite.ShopSupplies.MinimumCharge;
            saleCode.ShopSupplies.MinimumJobAmount = saleCodeToWrite.ShopSupplies.MinimumJobAmount;
            saleCode.ShopSupplies.Percentage = saleCodeToWrite.ShopSupplies.Percentage;
        }
    }
}
