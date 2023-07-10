using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
{
    public enum ExpenseCategory
    {
        [Display(Name = "Cost of Direct Labor")]
        CostOfDirectLabor = 0,

        [Display(Name = "Cost of Sales")]
        CostOfSales = 1,

        [Display(Name = "Other/Fixed Operating Expense")]
        OtherFixedOperatingExpense
    }
}
