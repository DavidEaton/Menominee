﻿using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Businesses
{
    public class BusinessToWrite
    {
        public long Id { get; set; }
        public BusinessNameRequest Name { get; set; } = new();

        /*
         * If you define your navigation property virtual, Entity Framework will at runtime create a new class (dynamic proxy) derived from your class and uses it instead of your original class. This new dynamically created class contains logic to load the navigation property when accessed for the first time. This is referred to as "lazy loading". It enables Entity Framework to avoid loading an entire tree of dependent objects which are not needed from the database.
         *
         * VK: All criticism of the lazy loading boils down to performance issues and the N+1 problem
         * Lazy loading is beneficial in writes (in terms of performance and simplicity)
         * Lazy loading is harmful only in reads
         * The drawbacks of the lazy loading can be overcome by the adherence to CQRS: use lazy loading only in writes, handwrite SQL queries in reads
         */
        public PersonToWrite Contact { get; set; } = new();
        public AddressToWrite Address { get; set; } = new();
        public string Notes { get; set; } = string.Empty;
        public List<PhoneToWrite> Phones { get; set; } = new List<PhoneToWrite>();
        public List<EmailToWrite> Emails { get; set; } = new List<EmailToWrite>();
        public bool IsEmpty =>
            Id == 0 &&
            (Name == null || string.IsNullOrWhiteSpace(Name.ToString())) &&
            (Address == null || Address.IsEmpty) &&
            (Phones == null || Phones.Count == 0) &&
            (Emails == null || Emails.Count == 0);
        public bool IsNotEmpty => !IsEmpty;
    }
}
