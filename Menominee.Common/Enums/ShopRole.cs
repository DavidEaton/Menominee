using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums
{
    public enum ShopRole
    {
        Admin,
        Advisor,
        Employee,
        [Display(Name = "Human Resources")]
        HumanResources,
        Manager,
        Owner,
        Technician
    }
}
