using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Interfaces
{
    public interface IContactLists
    {
        IList<Phone> Phones { get; }
        IList<Email> Emails { get; }
    }
}
