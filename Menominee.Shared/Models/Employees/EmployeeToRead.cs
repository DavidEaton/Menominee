using Menominee.Common.Enums;
using System;

namespace Menominee.Shared.Models.Employees;

public class EmployeeToRead
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public DateTime? Hired { get; set; }
    public string ShopRole { get; set; }
    public string SSN { get; set; }
    public string CertificationNumber { get; set; }
    public bool Active { get; set; }
    public string PrintedName { get; set; }
    public EmployeeExpenseCategory ExpenseCategory { get; set; }
    public double BenefitLoad { get; set; }
}
