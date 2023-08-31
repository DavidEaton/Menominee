using Menominee.Domain.Entities;

namespace Menominee.Shared.Models.Employees
{
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
                    Hired = employee.Hired
                };
        }
    }
}
