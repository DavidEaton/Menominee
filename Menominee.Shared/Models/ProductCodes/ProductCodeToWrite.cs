using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.SaleCodes;

namespace Menominee.Shared.Models.ProductCodes
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
