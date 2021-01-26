using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;

namespace Client.Models
{
    public class CustomerFlatDto
    {
        public int Id { get; set; }
        public IEntity Entity { get; set; }
        public EntityType EntityType { get; set; }
        public int EntityId { get; set; }
        public CustomerType CustomerType { get; set; }

        // TODO: Factory method to contruct; make constructor inaccessible
        public string Name { get; set; }


        // Flattened Address type
        public string AddressLine { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressPostalCode { get; set; }


        public bool AllowMail { get; set; }
        public bool AllowEmail { get; set; }
        public bool AllowSms { get; set; }
        public int PriceProfileId { get; set; }
        public int TaxIds { get; set; }
        public bool RewardsMember { get; set; }
        public bool OverrideCustomerTaxProfile { get; set; }
        public DateTime Created { get; set; }
        //public IList<Phone> Phones { get; set; } = new List<Phone>();
        //public IList<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
    }
}
