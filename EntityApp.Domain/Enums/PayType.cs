using System.ComponentModel.DataAnnotations;

namespace EntityApp.Domain.Enums
{
    public enum PayType
    {
        [Display(Name = "Hourly Only")]
        HourlyOnly = 0,

        [Display(Name = "Salary Only")]
        SalaryOnly = 1,

        Commission = 2,

        [Display(Name = "Salary Plus Commission")]
        SalaryPlusCommission = 3,

        [Display(Name = "Hourly Plus Commission")]
        HourlyPlusCommission = 4
    }
}
