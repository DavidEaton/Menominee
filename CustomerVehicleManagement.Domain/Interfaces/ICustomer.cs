using CustomerVehicleManagement.Domain.Entities;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Interfaces
{
    // VK: Interfaces for domain classes is a bad idea. Here are a couple of relevant sections from my book on this topic:
    // https://i.imgur.com/IA2jBpy.png
    // https://i.imgur.com/auGHL30.png
    
    public interface ICustomer
    {
        CustomerType CustomerType { get; }
        ContactPreferences ContactPreferences { get; }
        DateTime Created { get; }
        IList<Vehicle> Vehicles { get; }
    }
}