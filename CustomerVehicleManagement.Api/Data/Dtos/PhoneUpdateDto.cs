﻿using SharedKernel.Enums;

namespace CustomerVehicleManagement.Api.Data.Dtos
{
    public class PhoneUpdateDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public PhoneType PhoneType { get; set; }
        public bool IsPrimary { get; set; }
    }
}