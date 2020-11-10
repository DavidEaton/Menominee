using System;

namespace EntityApp.Domain.Classes
{
    public class Employee : Party, IEntity
    {
        public Person Person { get; set; }

        // Enitity Framework convenience property
        public int PersonId { get; set; }
        public DateTime Hired { get; set; }
    }
}
