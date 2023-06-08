using CustomerVehicleManagement.Domain.Enums;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Techs
{
    public class RoleAssignmentToWrite
    {
        public EmploymentRole Role { get; set; }
        public bool IsActive { get; set; }
    }
}
