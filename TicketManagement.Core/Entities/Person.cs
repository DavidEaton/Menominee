using SharedKernel;
using System;

namespace TicketManagement.Core.Entities
{
    public class Person : Entity
    {
        public string FirstName { get; set; }
        public string MiddelName { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        //Flatten the value objects
        //public DriversLicense DriversLicense { get; set; }
        //public Address Address { get; set; }

        #region ORM

        // EF requires an empty constructor
        protected Person() { }

        #endregion

    }
}
