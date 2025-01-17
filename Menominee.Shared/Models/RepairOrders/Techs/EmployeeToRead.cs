﻿using Menominee.Domain.Entities;
using Menominee.Shared.Models.Persons;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.RepairOrders.Techs
{
    public class EmployeeToRead
    {
        public long Id { get; set; }
        public PersonToRead PersonalDetails { get; set; }
        public DateTime? Hired { get; set; }
        public DateTime? Exited { get; set; }
        public IList<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();
    }
}
