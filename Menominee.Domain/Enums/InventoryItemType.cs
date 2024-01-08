using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
{
    public enum InventoryItemType
    {
        Part,
        Labor,
        Tire,
        Package,
        Inspection,
        Donation,
        Warranty,
        [Display(Name = "Gift Certificate")]
        GiftCertificate
    }
}
