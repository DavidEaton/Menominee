using CustomerVehicleManagement.Domain.Entities;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Interfaces
{
    public interface ICustomer
    {
        IEntity Entity { get; }
        int EntityId { get; }
        CustomerType CustomerType { get; }
        ContactPreferences ContactPreferences { get; }
        DateTime Created { get; }
        IList<Vehicle> Vehicles { get; }

        void SetEntity(Entity entity);
    }
}