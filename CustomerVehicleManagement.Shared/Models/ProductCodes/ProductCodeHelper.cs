using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeHelper
    {
        public static ProductCodeToReadInList CreateProductCodeInList(ProductCode productCode)
        {
            if (productCode is not null)
            {
                return new ProductCodeToReadInList
                {
                    Id = productCode.Id,
                    Manufacturer = productCode.Manufacturer,
                    Code = productCode.Code,
                    SaleCode = productCode.SaleCode,
                    Name = productCode.Name
                };
            }

            return null;
        }

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
