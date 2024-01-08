using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
{
    public enum InventoryItemInspectionType
    {
        [Display(Name = "Courtesy Check")]
        CourtesyCheck,
        Paid
    }
}
