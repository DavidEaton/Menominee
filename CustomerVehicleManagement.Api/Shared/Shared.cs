using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api
{
    public static class Shared
    {
        public static Organization CreateOrganizationToAdd(OrganizationToWrite organizationToAdd)
        {
            Address organizationAddress = null;
            List<Phone> phones = new();
            List<Email> emails = new();

            // FluentValidation has already validated request; no need to validate Name again here
            var organizationName = OrganizationName.Create(organizationToAdd.Name).Value;

            if (organizationToAdd?.Address != null)
                organizationAddress = Address.Create(
                    organizationToAdd.Address.AddressLine,
                    organizationToAdd.Address.City,
                    organizationToAdd.Address.State,
                    organizationToAdd.Address.PostalCode).Value;

            if (organizationToAdd?.Phones?.Count > 0)
                // FluentValidation has already validated contactable collections
                foreach (var phone in organizationToAdd.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (organizationToAdd?.Emails?.Count > 0)
                foreach (var email in organizationToAdd.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            //Organization.Contact
            Person person = null;
            Address personAddress = null;
            DriversLicense driversLicense = null;

            if (organizationToAdd?.Contact != null)
            {
                if (organizationToAdd?.Contact?.Address != null)
                    personAddress = Address.Create(
                        organizationToAdd.Contact.Address.AddressLine,
                        organizationToAdd.Contact.Address.City,
                        organizationToAdd.Contact.Address.State,
                        organizationToAdd.Contact.Address.PostalCode).Value;

                if (organizationToAdd?.Contact?.DriversLicense != null)
                {
                    DateTimeRange dateTimeRange = DateTimeRange.Create(
                        organizationToAdd.Contact.DriversLicense.Issued,
                        organizationToAdd.Contact.DriversLicense.Expiry).Value;

                    driversLicense = DriversLicense.Create(organizationToAdd.Contact.DriversLicense.Number,
                        organizationToAdd.Contact.DriversLicense.State,
                        dateTimeRange).Value;
                }

                person = new Person(
                PersonName.Create(
                    organizationToAdd.Contact.Name.LastName,
                    organizationToAdd.Contact.Name.FirstName,
                    organizationToAdd.Contact.Name.MiddleName).Value,
                organizationToAdd.Contact.Gender,
                personAddress, emails, phones,
                organizationToAdd.Contact.Birthday,
                driversLicense);
            }

            var organization = new Organization(organizationName, organizationToAdd.Note, person, organizationAddress, emails, phones);
            return organization;
        }

    }
}
