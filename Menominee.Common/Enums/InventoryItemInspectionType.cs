using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum InventoryItemInspectionType
    {
        [Display(Name = "Courtesy Check")]
        CourtesyCheck,
        Paid
    }
}
