﻿using Menominee.Shared.Models.Persons;
using Menominee.Common.Enums;

namespace Menominee.Shared.Models.Customers
{
    public class CustomerToWrite
    {
        public EntityType EntityType { get; set; }
        public CustomerType CustomerType { get; set; }
        public PersonToWrite Person { get; set; }
        public OrganizationToWrite Organization { get; set; }

        //public ContactPreferences ContactPreferences { get; set; }
    }
}