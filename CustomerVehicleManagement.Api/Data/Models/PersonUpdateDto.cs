using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Data.Models
{
    public class PersonUpdateDto
    {
        public int Id { get; set; }
        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public Address Address { get; set; }
        public IList<PhoneUpdateDto> Phones { get; set; } = new List<PhoneUpdateDto>();
    }

}
