using Menominee.Common;
using System;

namespace CustomerVehicleManagement.Domain.Entities
{
    class Employee : Entity
    {
        public Person Person { get; private set; }
        public DateTime Hired { get; private set; }

        public Employee(Person person)
        {
            Person = person;
            Hired = DateTime.Today;
        }

    }
}
