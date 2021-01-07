using Migrations.Core.Entities;
using Migrations.Core.Enums;
using Migrations.Core.ValueObjects;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Migrations.Core.Interfaces
{
    public interface ICustomer
    {
        IEntity Entity { get; set; }
        int EntityId { get; set; }
        CustomerType CustomerType { get; set; }
        Address Address { get; set; }
        bool AllowEmail { get; set; }
        bool AllowMail { get; set; }
        bool AllowSms { get; set; }
        bool OverrideCustomerTaxProfile { get; set; }
        int PriceProfileId { get; set; }
        bool RewardsMember { get; set; }
        int TaxIds { get; set; }
        DateTime Created { get; set; }
        DateTime? ValidThru { get; set; }
        ICollection<Vehicle> Vehicles { get; }
    }
}