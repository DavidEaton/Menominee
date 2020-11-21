using System.ComponentModel.DataAnnotations;

namespace Migrations.Core.Enums
{
    public enum EmploymentRole
    {
        Sales = 0,
        Technician = 1,
        Inspector = 2,
        Counter = 3,

        [Display(Name = "Service Advisor")]
        ServiceAdvisor
    }
}
