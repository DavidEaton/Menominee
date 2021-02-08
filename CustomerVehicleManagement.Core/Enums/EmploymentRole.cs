﻿using System.ComponentModel.DataAnnotations;

namespace CustomerVehicleManagement.Domain.Enums
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