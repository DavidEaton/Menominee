using CustomerVehicleManagement.Domain.Entities;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Interfaces
{
    public interface ICustomer
    {
        IEntity Entity { get; }
        int EntityId { get; }
        CustomerType CustomerType { get; }
        bool AllowEmail { get; }
        bool AllowMail { get; }
        bool AllowSms { get; }
        bool OverrideCustomerTaxProfile { get; }
        int PriceProfileId { get; }
        bool RewardsMember { get; }
        int TaxIds { get; }
        DateTime Created { get; }
        IList<Vehicle> Vehicles { get; }

        void SetEntity(Entity entity);
    }
}