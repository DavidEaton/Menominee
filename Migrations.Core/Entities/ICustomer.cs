using Migrations.Core.Enums;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Migrations.Core.Entities
{
    public interface ICustomer
    {
        int Id { get; set; }
        IEntity Entity { get; set; }
        int EntityId { get; set; }
        EntityType EntityType { get; set; }
        CustomerType CustomerType { get; set; }
        Address Address { get; set; }
        bool AllowEmail { get; set; }
        bool AllowMail { get; set; }
        bool AllowSms { get; set; }
        bool OverrideCustomerTaxProfile { get; set; }
        int PriceProfileId { get; set; }
        bool RewardsMember { get; set; }
        int TaxIds { get; set; }
        DateTime ValidFrom { get; set; }
        DateTime? ValidThru { get; set; }
        ICollection<Vehicle> Vehicles { get; }
        ICollection<Phone> Phones { get; }

        TrackingState TrackingState { get; }
        void UpdateState(TrackingState state);
    }
}