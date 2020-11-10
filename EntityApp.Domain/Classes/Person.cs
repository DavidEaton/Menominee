using EntityApp.Domain.Enums;
using System;
using System.Collections.Generic;

namespace EntityApp.Domain.Classes
{
    public class Person : Party, IEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Birthday { get; set; }
        public Gender Gender { get; set; }

    }
}
