using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.RepairOrders.Techs
{
    public class RoleAssignmentToWrite
    {
        public EmploymentRole Role { get; set; }
        public bool IsActive { get; set; }
    }
}
