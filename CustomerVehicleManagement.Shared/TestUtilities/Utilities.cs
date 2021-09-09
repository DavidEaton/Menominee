﻿using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.TestUtilities
{
    public static class Utilities
    {
        public static Organization CreateValidOrganization()
        {
            var name = "jane's";
            Organization organization = null;
            var organizationNameOrError = OrganizationName.Create(name);

            if (organizationNameOrError.IsSuccess)
                organization = new Organization(organizationNameOrError.Value);

            return organization;
        }

        public static Customer CreateValidOrganizationCustomer()
        {
            var name = "jane's";
            Organization organization = null;
            var organizationNameOrError = OrganizationName.Create(name);

            if (organizationNameOrError.IsSuccess)
                organization = new Organization(organizationNameOrError.Value);

            var customer = new Customer(organization);

            return customer;
        }

        public static Customer CreateValidPersonCustomer()
        {
            var person = CreateValidPerson();

            var customer = new Customer(person);

            return customer;
        }

        public static Person CreateValidPerson()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var nameOrError = PersonName.Create(lastName, firstName);

            return new Person(nameOrError.Value, Gender.Female);
        }

        public static Person CreateValidPersonWithPhones()
        {
            var person = CreateValidPerson();
            person.SetPhones(CreateValidPhones());
            return person;
        }

        public static Person CreateValidPersonWithEmails()
        {
            var person = CreateValidPerson();
            person.SetEmails(CreateValidEmails());
            return person;
        }

        public static IList<Phone> CreateValidPhones()
        {
            IList<Phone> phones = new List<Phone>();

            var number1 = "(555) 987-6543";
            var phoneType1 = PhoneType.Mobile;
            var isPrimary1 = true;
            var phone1 = new Phone(number1, phoneType1, isPrimary1);

            var number2 = "(555) 123-4567";
            var phoneType2 = PhoneType.Mobile;
            var isPrimary2 = false;
            var phone2 = new Phone(number2, phoneType2, isPrimary2);

            phones.Add(phone1);
            phones.Add(phone2);

            return phones;
        }

        public static IList<Domain.Entities.Email> CreateValidEmails()
        {
            var Emails = new List<Domain.Entities.Email>();

            var address1 = "a@b.c";
            var isPrimary1 = true;
            var Email1 = new Domain.Entities.Email(address1, isPrimary1);

            var address2 = "d@e.f";
            var isPrimary2 = false;
            var Email2 = new Domain.Entities.Email(address2, isPrimary2);

            Emails.Add(Email1);
            Emails.Add(Email2);

            return Emails;
        }

        public static IReadOnlyList<EmailReadDto> CreateValidEmailReadDtos()
        {
            List<EmailReadDto> Emails = new();

            var address1 = "a@b.c";
            var isPrimary1 = true;
            var Email1 = new EmailReadDto
            {
                Address = address1,
                IsPrimary = isPrimary1
            };

            var address2 = "d@e.f";
            var isPrimary2 = false;
            var Email2 = new EmailReadDto
            {
                Address = address2,
                IsPrimary = isPrimary2
            };

            Emails.Add(Email1);
            Emails.Add(Email2);

            IReadOnlyList<EmailReadDto> readOnlyEmails = Emails;
            return readOnlyEmails;
        }

        public static Address CreateValidAddress()
        {
            string addressLine = "1234 Fifth Ave.";
            string city = "Traverse City";
            State state = State.MI;
            string postalCode = "49686";

            var addressOrError = Address.Create(addressLine, city, state, postalCode);
            return addressOrError.Value;
        }

    }
}