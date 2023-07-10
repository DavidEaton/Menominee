using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.SaleCodes;

namespace Menominee.Shared.Models.ProductCodes
{
    public class ProductCodeToReadInList
    {
        public long Id { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string Code { get; set; }
        public SaleCodeToRead SaleCode { get; set; }
        public string Name { get; set; }
        public string DisplayText
        {
            get
            {
                return Code + " - " + Name;
            }
        }
    }
}
