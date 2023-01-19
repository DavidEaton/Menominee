using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.SaleCodes;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeToWrite
    {
        public long Id { get; set; }
        public ManufacturerToWrite Manufacturer { get; set; }
        public string Code { get; set; }
        public SaleCodeToWrite SaleCode { get; set; }
        public string Name { get; set; }

    }
}
