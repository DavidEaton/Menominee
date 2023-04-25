using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using System;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeHelper
    {
        public static ProductCodeToReadInList ConvertEntityToReadInListDto(ProductCode productCode)
        {
            return productCode is null
                ? new ProductCodeToReadInList()
                : new()
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
            return productCode is null
                ? new ProductCodeToRead()
                : new()
                {
                    Id = productCode.Id,
                    Code = productCode.Code,
                    Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(productCode.Manufacturer),
                    Name = productCode.Name,
                    SaleCode = SaleCodeHelper.ConvertEntityToReadDto(productCode.SaleCode)
                };
        }

        public static ProductCodeToWrite ConvertReadToWriteDto(ProductCodeToRead productCode)
        {
            return productCode is null
                ? new ProductCodeToWrite()
                : new()
                {
                    Id = productCode.Id,
                Code = productCode.Code,
                Manufacturer = ManufacturerHelper.ConvertReadToWriteDto(productCode.Manufacturer),
                Name = productCode.Name,
                SaleCode = SaleCodeHelper.ConvertReadToWriteDto(productCode.SaleCode)
            };
        }

        public static ProductCodeToRead ConvertToWriteDto(ProductCode productCode)
        {
            return productCode is null
                ? new ProductCodeToRead()
                : new()
                {
                    Id = productCode.Id,
                    Code = productCode.Code,
                    Manufacturer = ManufacturerHelper.ConvertEntityToReadDto(productCode.Manufacturer),
                    Name = productCode.Name,
                    SaleCode = SaleCodeHelper.ConvertEntityToReadDto(productCode.SaleCode)
                };
        }
    }
}
