﻿using System.ComponentModel.DataAnnotations;

namespace Menominee.Common.Enums;

public enum EmployeeExpenseCategory
{
    [Display(Name = "Cost of Direct Labor")]
    CostOfDirectLabor,

    [Display(Name = "Cost of Sales")]
    CostOfSales,

    [Display(Name = "Other/Fixed Operating Expenses")]
    OtherOrFixedOperatingExpenses,
}
