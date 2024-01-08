using Menominee.Domain.Enums;
using Menominee.Shared.Models.Persons;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.RepairOrders.Techs;

public class EmployeeToWrite
{
    public PersonToWrite PersonalDetails { get; set; }
    public DateTime? Hired { get; set; }
    public DateTime? Exited { get; set; }
    public IList<RoleAssignmentToWrite> RoleAssignments { get; set; } = new List<RoleAssignmentToWrite>();
    public string SSN { get; set; }
    public string CertificationNumber { get; set; }
    public bool Active { get; set; } = true;
    public string PrintedName { get; set; }
    public EmployeeExpenseCategory ExpenseCategory { get; set; } = EmployeeExpenseCategory.CostOfDirectLabor;
    public double BenefitLoad { get; set; } = 0.0;
}
