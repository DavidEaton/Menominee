using Menominee.Common;
using Menominee.Common.Enums;
using Menominee.Common.Utilities;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItem : Entity
    {
        public Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public ProductCode ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public InventoryItemType ItemType { get; set; }

        public InventoryItemPart Part { get; set; }
        public InventoryItemLabor Labor { get; set; }
        public InventoryItemTire Tire { get; set; }
        public InventoryItemPackage Package { get; set; }
        public InventoryItemCourtesyCheck CourtesyCheck { get; set; }
        //public InventoryItemDonation Donation { get; set; }
        //public InventoryItemGiftCertificate GiftCertificate { get; set; }

        public InventoryItem(InventoryItemPart part)
        {
            Guard.ForNull(part, "part == null");

            Part = part;
            ItemType = InventoryItemType.Part;
        }

        public InventoryItem(InventoryItemLabor labor)
        {
            Guard.ForNull(labor, "labor == null");

            Labor = labor;
            ItemType = InventoryItemType.Labor;
        }

        public InventoryItem(InventoryItemTire tire)
        {
            Guard.ForNull(tire, "tire == null");

            Tire = tire;
            ItemType = InventoryItemType.Tire;
        }

        public InventoryItem(InventoryItemPackage package)
        {
            Guard.ForNull(package, "package == null");

            Package = package;
            ItemType = InventoryItemType.Package;
        }

        public InventoryItem(InventoryItemCourtesyCheck courtesyCheck)
        {
            Guard.ForNull(courtesyCheck, "courtesyCheck == null");

            CourtesyCheck = courtesyCheck;
            ItemType = InventoryItemType.CourtesyCheck;
        }

        //public InventoryItem(InventoryItemDonation donation)
        //{
        //    Guard.ForNull(donation, "donation == null");

        //    Donation = donation;
        //    ItemType = InventoryItemType.Donation;
        //}

        //public InventoryItem(InventoryItemGiftCertificate giftCertificate)
        //{
        //    Guard.ForNull(giftCertificate, "giftCertificate == null");

        //    GiftCertificate = giftCertificate;
        //    ItemType = InventoryItemType.GiftCertificate;
        //}

        #region ORM

        // EF requires an empty constructor
        public InventoryItem() { }

        #endregion
    }
}
