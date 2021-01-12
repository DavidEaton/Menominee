using CustomerVehicleManagement.Domain.Interfaces;
using SharedKernel;
using System;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Employee : Entity, IEmployee
    {
        private DateTime? Terminated;

        // EF requires an empty constructor
        protected Employee() { }

        public Employee(Person person, DateTime hired)
        {
            Person = person;
            Hired = hired;
        }

        public Person Person { get; set; }

        // Enitity Framework convenience property
        public int PersonId { get; set; }
        public DateTime Hired { get; set; }

        public void Terminate(DateTime terminated)
        {
            SetTerminated(terminated);
        }

        public DateTime? GetTerminated()
        {
            return Terminated;
        }

        private void SetTerminated(DateTime? terminated)
        {
            Terminated = terminated;
        }

        public bool Active { get => !GetTerminated().HasValue; }
    }
}
