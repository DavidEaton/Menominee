﻿using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.SaleCodes;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeHelper
    {
        public static ProductCodeToReadInList ConvertEntityToReadInListDto(ProductCode productCode)
        {
            if (productCode is null)
                return null;

            return new()
            {
                Id = productCode.Id,
                Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(productCode.Manufacturer),
                Code = productCode.Code,
                SaleCode = SaleCodeHelper.ConvertEntityToReadDto(productCode.SaleCode),
                Name = productCode.Name
            };
        }

        public static ProductCodeToRead ConvertEntityToReadDto(ProductCode productCode)
        {
            if (productCode == null)
                return new ProductCodeToRead();

            return new()
            {
                Id = productCode.Id,
                Code = productCode.Code,
                Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(productCode.Manufacturer),
                Name = productCode.Name,
                SaleCode = SaleCodeHelper.ConvertEntityToReadDto(productCode.SaleCode)
            };
        }

        public static ProductCode ConvertWriteDtoToEntity(ProductCodeToWrite productCode)
        {
            if (productCode == null)
                return new ProductCode();

            return ProductCode.Create(
                ManufacturerHelper.ConvertWriteDtoToEntity(productCode.Manufacturer),
                productCode.Code,
                productCode.Name,
                SaleCodeHelper.ConvertWriteDtoToEntity(productCode.SaleCode)).Value;
        }

        public static ProductCodeToWrite ConvertReadToWriteDto(ProductCodeToRead productCode)
        {
            if (productCode == null)
                return new ProductCodeToWrite();

            return new()
            {
                Id = productCode.Id,
                Code = productCode.Code,
                Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(productCode.Manufacturer),
                Name = productCode.Name,
                SaleCode = SaleCodeHelper.ConvertReadToWriteDto(productCode.SaleCode)
            };
        }

        public static void CopyWriteDtoToEntity(ProductCodeToWrite productCodeToUpdate, ProductCode productCode)
        {
            productCode.SetCode(productCodeToUpdate.Code);
            productCode.SetManufacturer(ManufacturerHelper.ConvertWriteDtoToEntity(productCodeToUpdate.Manufacturer));
            productCode.SetName(productCodeToUpdate.Name);
            productCode.SetSaleCode(SaleCodeHelper.ConvertWriteDtoToEntity(productCodeToUpdate.SaleCode));
        }
    }
}
