using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Interfaces
{
    public interface IContactLists
    {
        IReadOnlyList<Phone> Phones { get; }
        IReadOnlyList<Email> Emails { get; }
    }
}
