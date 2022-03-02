using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class OrganizationHelper
    {
        public static Organization CreateEntityFromWriteDto(OrganizationToWrite organization)
        {
            Address organizationAddress = null;
            List<Phone> phones = new();
            List<Email> emails = new();
            OrganizationName organizationName;

            organizationName = OrganizationName.Create(organization.Name).Value;
            if (organization?.Address != null)
                organizationAddress = Address.Create(
                    organization.Address.AddressLine,
                    organization.Address.City,
                    organization.Address.State,
                    organization.Address.PostalCode).Value;

            if (organization?.Phones?.Count > 0)
                foreach (var phone in organization.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (organization?.Emails?.Count > 0)
                foreach (var email in organization.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            //Organization.Contact
            Person person = null;
            Address personAddress = null;
            DriversLicense driversLicense = null;

            if (organization?.Contact != null)
            {
                if (organization?.Contact?.Address != null)
                    personAddress = Address.Create(
                        organization.Contact.Address.AddressLine,
                        organization.Contact.Address.City,
                        organization.Contact.Address.State,
                        organization.Contact.Address.PostalCode).Value;

                if (organization?.Contact?.DriversLicense != null)
                {
                    DateTimeRange dateTimeRange = DateTimeRange.Create(
                        organization.Contact.DriversLicense.Issued,
                        organization.Contact.DriversLicense.Expiry).Value;

                    driversLicense = DriversLicense.Create(organization.Contact.DriversLicense.Number,
                        organization.Contact.DriversLicense.State,
                        dateTimeRange).Value;
                }

                person = new Person(
                PersonName.Create(
                    organization.Contact.Name.LastName,
                    organization.Contact.Name.FirstName,
                    organization.Contact.Name.MiddleName).Value,
                organization.Contact.Gender,
                personAddress, emails, phones,
                organization.Contact.Birthday,
                driversLicense);
            }

            return new Organization(organizationName, organization.Note, person, organizationAddress, emails, phones);

        }

        public static OrganizationToWrite CreateWriteDtoFromReadDto(OrganizationToRead organization)
        {
            OrganizationToWrite Organization = new()
            {
                Name = organization.Name,
                Note = organization?.Note,
            };

            if (organization?.Address is not null)
                organization.Address = new()
                {
                    AddressLine = organization.Address.AddressLine,
                    City = organization.Address.City,
                    State = organization.Address.State,
                    PostalCode = organization.Address.PostalCode
                };

            if (organization?.Contact != null)
            {
                Organization.Contact = new()
                {
                    Name = new()
                    {
                        LastName = organization.Contact.LastName,
                        MiddleName = organization.Contact.MiddleName,
                        FirstName = organization.Contact.FirstName
                    },

                    Gender = organization.Contact.Gender,
                    Birthday = organization.Contact?.Birthday

                };

                if (organization.Contact?.Address is not null)
                    Organization.Contact.Address = new()
                    {
                        AddressLine = organization.Contact.Address.AddressLine,
                        City = organization.Contact.Address.City,
                        State = organization.Contact.Address.State,
                        PostalCode = organization.Contact.Address.PostalCode
                    };


                if (organization.Contact?.DriversLicense is not null)
                    Organization.Contact.DriversLicense = new()
                    {
                        Number = organization.Contact.DriversLicense.Number,
                        State = organization.Contact.DriversLicense.State,
                        Issued = organization.Contact.DriversLicense.Issued,
                        Expiry = organization.Contact.DriversLicense.Expiry
                    };

                if (organization.Contact?.Phones.Count > 0)
                {
                    foreach (var phone in organization.Contact.Phones)
                    {
                        Organization.Contact.Phones.Add(new()
                        {
                            Number = phone.Number,
                            PhoneType = Enum.Parse<PhoneType>(phone.PhoneType),
                            IsPrimary = phone.IsPrimary
                        });
                    }
                }

                if (Organization.Contact?.Emails.Count > 0)
                {
                    foreach (var email in organization.Contact.Emails)
                    {
                        Organization.Contact.Emails.Add(new()
                        {
                            Address = email.Address,
                            IsPrimary = email.IsPrimary
                        });
                    }
                }
            }

            return Organization;
        }
    }
}
