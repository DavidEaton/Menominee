using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
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
