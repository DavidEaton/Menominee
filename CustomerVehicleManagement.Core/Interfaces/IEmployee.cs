using CustomerVehicleManagement.Domain.Entities;
using System;

namespace CustomerVehicleManagement.Domain.Interfaces
{
    public interface IEmployee
    {
        bool Active { get; }
        DateTime Hired { get; set; }
        Person Person { get; set; }
        int PersonId { get; set; }
        DateTime? GetTerminated();
        void Terminate(DateTime terminated);
    }
}