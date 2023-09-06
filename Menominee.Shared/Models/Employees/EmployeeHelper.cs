using Menominee.Domain.Entities;

namespace Menominee.Shared.Models.Employees;

public class EmployeeHelper
{
    public static EmployeeToRead ConvertToReadDto(Employee employee)
    {
        return
        employee is null
            ? null
            : new()
            {
                Id = employee.Id,
                Name = employee.PersonalDetails.Name.ToString(),
                Gender = employee.PersonalDetails.Gender,
                Hired = employee.Hired,
                SSN = employee.SSN,
                CertificationNumber = employee.CertificationNumber,
                Active = employee.Active,
                PrintedName = employee.PrintedName,
                ExpenseCategory = employee.ExpenseCategory,
                BenefitLoad = employee.BenefitLoad
            };
    }
}
