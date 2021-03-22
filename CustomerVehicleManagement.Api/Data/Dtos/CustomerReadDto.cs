using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Data.Dtos
{
    public class CustomerReadDto
    {
        public int Id { get; set; }
        public EntityType EntityType { get; set; }

        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public Address Address { get; set; }
        public PersonReadDto Contact { get; set; }
        public CustomerType CustomerType { get; set; }
        public bool AllowMail { get; set; }
        public bool AllowEmail { get; set; }
        public bool AllowSms { get; set; }
        public int PriceProfileId { get; set; }
        public int TaxIds { get; set; }
        public bool RewardsMember { get; set; }
        public bool OverrideCustomerTaxProfile { get; set; }
        public IList<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
    }
}
