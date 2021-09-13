using System.ComponentModel.DataAnnotations;

namespace SharedKernel.Enums
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
