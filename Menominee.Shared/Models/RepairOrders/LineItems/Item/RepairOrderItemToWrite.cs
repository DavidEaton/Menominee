using Menominee.Common.Enums;
using Menominee.Shared.Models.Manufacturers;
using Menominee.Shared.Models.ProductCodes;
using Menominee.Shared.Models.SaleCodes;

namespace Menominee.Shared.Models.RepairOrders.LineItems.Item
{
    public class RepairOrderItemToWrite
    {
        public long Id { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public SaleCodeToRead SaleCode { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public PartType PartType { get; set; } = PartType.Part;
        public RepairOrderItemPartToWrite Part { get; set; } //optional
        public RepairOrderItemLaborToWrite Labor { get; set; } //optional
    }
}
