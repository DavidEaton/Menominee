using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeHelper
    {
        public static ProductCodeToReadInList CreateProductCodeInList(ProductCode productCode)
        {
            if (productCode is null)
                return null;

            return new()
            {
                Id = productCode.Id,
                Manufacturer = productCode.Manufacturer,
                Code = productCode.Code,
                SaleCode = productCode.SaleCode,
                Name = productCode.Name
            };
        }

        public static ProductCodeToRead CreateProductCode(ProductCode productCode)
        {
            if (productCode == null)
                return new ProductCodeToRead();

            return new()
            {
                Id = productCode.Id,
                Code = productCode.Code,
                Manufacturer = productCode.Manufacturer,
                Name = productCode.Name,
                SaleCode = productCode.SaleCode
            };
        }

        public static ProductCodeToWrite CreateProductCode(ProductCodeToRead productCode)
        {
            if (productCode == null)
                return new ProductCodeToWrite();

            return new()
            {
                Code = productCode.Code,
                Manufacturer = productCode.Manufacturer,
                Name = productCode.Name,
                SaleCode = productCode.SaleCode
            };
        }

        public static ProductCode CreateProductCode(ProductCodeToWrite productCode)
        {
            if (productCode == null)
                return null;

            return new()
            {
                Code = productCode.Code,
                Manufacturer = productCode.Manufacturer,
                Name = productCode.Name,
                SaleCode = productCode.SaleCode
            };
        }
    }
}
