using Migrations.Core.Entities;
using Migrations.Core.Enums;
using Migrations.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace Migrations.Core.Interfaces
{
    public interface IPerson
    {
        Address Address { get; set; }
        DateTime? Birthday { get; set; }
        DriversLicence DriversLicence { get; set; }
        string FirstName { get; set; }
        Gender Gender { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
        string NameFirstLast { get; }
        string NameLastFirst { get; }
        ICollection<Phone> Phones { get; set; }
    }
}