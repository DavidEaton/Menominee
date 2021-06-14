using System;
using System.Collections.Generic;
using SharedKernel.Enums;

namespace Menominee.UiExperiments.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public EntityType EntityType { get; set; }
        public CustomerType CustomerType { get; set; }
        public string Name { get; set; }
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
        public IList<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
        public IList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        #region ORM

        // EF requires an empty constructor
        protected Customer() { }

        #endregion

    }
}
