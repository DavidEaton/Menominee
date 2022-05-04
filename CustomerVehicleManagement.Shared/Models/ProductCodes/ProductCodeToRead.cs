using CustomerVehicleManagement.Domain.Entities;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeToRead
    {
        public long Id { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public string Code { get; set; }
        public SaleCode SaleCode { get; set; }
        public string Name { get; set; }
    }
}
