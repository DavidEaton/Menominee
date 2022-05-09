using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Organizations
{
    public class OrganizationHelper
    {
        public static Organization CreateOrganization(OrganizationToWrite organization)
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

            return new Organization(organizationName,
                                    organization.Note,
                                    PersonHelper.CreateEntityFromWriteDto(organization?.Contact),
                                    organizationAddress,
                                    emails,
                                    phones);
        }

        public static OrganizationToWrite CreateOrganization(OrganizationToRead organization)
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
                Organization.Contact = PersonHelper.CreateWriteDtoFromReadDto(organization?.Contact);

            return Organization;
        }

        public static OrganizationToRead CreateOrganization(Organization organization)
        {
            if (organization != null)
            {
                return new OrganizationToRead()
                {
                    Id = organization.Id,
                    Name = organization.Name.Name,
                    Address = AddressHelper.ConvertToDto(organization.Address),
                    Note = organization.Note,
                    Phones = PhoneHelper.CreatePhones(organization.Phones),
                    Emails = EmailHelper.CreateEmails(organization.Emails),
                    Contact = PersonHelper.ConvertToReadDto(organization.Contact)
                };
            }

            return null;
        }

        public static OrganizationToReadInList CreateOrganizationInList(Organization organization)
        {
            if (organization != null)
            {
                return new OrganizationToReadInList
                {
                    Id = organization.Id,
                    Name = organization.Name.Name,
                    ContactName = organization?.Contact?.Name.LastFirstMiddle,
                    ContactPrimaryPhone = PhoneHelper.GetPrimaryPhone(organization?.Contact),

                    AddressLine = organization?.Address?.AddressLine,
                    City = organization?.Address?.City,
                    State = organization?.Address?.State.ToString(),
                    PostalCode = organization?.Address?.PostalCode,

                    Note = organization.Note,
                    PrimaryPhone = PhoneHelper.GetPrimaryPhone(organization),
                    PrimaryPhoneType = PhoneHelper.GetPrimaryPhoneType(organization),
                    PrimaryEmail = EmailHelper.GetPrimaryEmail(organization)
                };
            }

            return null;
        }
    }
}
