using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.LineItems.Item
{
    public class RepairOrderItemToRead
    {
        public ManufacturerToRead Manufacturer { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public SaleCodeToRead SaleCode { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public PartType PartType { get; set; }
    }
}
