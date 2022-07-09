using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToRead
    {
        public long Id { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public InventoryItemType ItemType { get; set; }
        public InventoryPartToRead Part { get; set; }
        public InventoryLaborToRead Labor { get; set; }
        public InventoryTireToRead Tire { get; set; }
        public InventoryPackageToRead Package { get; set; }
        public InventoryCourtesyCheckToRead CourtesyCheck { get; set; }
        // Coming soon...
        //public InventoryDonationToRead Donation { get; set; }
        //public InventoryGiftCertificateToRead GiftCertificate { get; set; }


        //public int QuantityOnHand { get; set; }
        //public double Cost { get; set; }
        //public double SuggestedPrice { get; set; }
        //public double Labor { get; set; }
    }
}
