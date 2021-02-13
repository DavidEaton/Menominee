using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Interfaces
{
    public interface IListOfPhone
    {
        IList<Phone> Phones { get; }
    }
}
