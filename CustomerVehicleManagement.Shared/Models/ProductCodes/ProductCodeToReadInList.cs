using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Entities.Inventory;

namespace CustomerVehicleManagement.Shared.Models.ProductCodes
{
    public class ProductCodeToReadInList
    {
        public long Id { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public string Code { get; set; }
        public SaleCode SaleCode { get; set; }
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
