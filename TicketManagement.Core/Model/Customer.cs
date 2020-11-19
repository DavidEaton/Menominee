using CustomerVehicleManagement.Core.Enums;
using CustomerVehicleManagement.Core.Model;
using System;
using System.Collections.Generic;

namespace TicketManagement.Core.Model
{
    public class Customer
    {
        public int Id { get; }
        public Person Person { get; }
        public Organization Organization { get; }
        public EntityType EntityType
        {
            get => (Organization != null) ? EntityType.Organization : EntityType.Person;

        }
        public CustomerType CustomerType { get; }
        public DateTime ValidFrom { get; }
        public IList<Vehicle> Vehicles { get; }
    }
}
