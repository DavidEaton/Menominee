using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeHelper
    {
        public static ProductCodeToRead CreateProductCode(ProductCode productCode)
        {
            if (productCode == null)
                return new ProductCodeToRead();

            return new ProductCodeToRead()
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

            return new ProductCodeToWrite()
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

            return new ProductCode()
            {
                Code = productCode.Code,
                Manufacturer = productCode.Manufacturer,
                Name = productCode.Name,
                SaleCode = productCode.SaleCode
            };
        }
    }
}
