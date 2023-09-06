using System.ComponentModel.DataAnnotations;

namespace Menominee.Domain.Enums;

public enum EmploymentRole
{
    [Display(Name = "Service Advisor")]
    ServiceAdvisor,

    [Display(Name = "Technician")]
    Technician,

    [Display(Name = "Inspector")]
    Inspector
}
