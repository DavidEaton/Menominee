using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums
{
    public enum EmploymentRole
    {
        Sales = 0,
        Technician = 1,
        Inspector = 2,
        Counter = 3,

        [Display(Name = "Service Advisor")]
        ServiceAdvisor = 4
    }
}
