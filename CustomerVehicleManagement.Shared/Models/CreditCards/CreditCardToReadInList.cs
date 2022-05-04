using CustomerVehicleManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.CreditCards
{
    public class CreditCardToReadInList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ProcessorName { get; set; }
    }
}
