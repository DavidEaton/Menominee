﻿using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class AddressAddDto
    {
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public State State { get; set; }
        public string PostalCode { get; set; } = string.Empty;
    }
}
