using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Enums;
using CustomerVehicleManagement.Domain.ValueObjects;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Interfaces
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
        IList<Vehicle> Vehicles { get; }
    }
}