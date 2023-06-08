using CustomerVehicleManagement.Shared.Models.Persons;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Techs
{
    public class EmployeeToWrite
    {
        public PersonToWrite PersonalDetails { get; set; }
        public DateTime? Hired { get; set; }
        public DateTime? Exited { get; set; }
        public IList<RoleAssignmentToWrite> RoleAssignments { get; set; } = new List<RoleAssignmentToWrite>();
    }
}
