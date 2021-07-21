using SharedKernel;
using SharedKernel.Utilities;
using System;

namespace CustomerVehicleManagement.Domain.Entities
{
    class Employee : Entity
    {
        public Person Person { get; private set; }
        public DateTime Hired { get; private set; }

        public Employee(Person person)
        {
            Guard.ForNull(person, "person == null");
            Person = person;
            Hired = DateTime.Today;
        }

    }
}
