using Migrations.Core.Enums;
using Migrations.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace Migrations.Core.Interfaces
{
    public interface IPerson
    {
        PersonName Name { get; set; }
        DateTime? Birthday { get; set; }
        Address Address { get; set; }
        DriversLicence DriversLicence { get; set; }
        Gender Gender { get; set; }
        IList<Phone> Phones { get; set; }
    }
}