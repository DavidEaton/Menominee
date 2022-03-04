using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.ValueObjects;
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

            return new Organization(organizationName,
                                    organization.Note,
                                    PersonHelper.CreateEntityFromWriteDto(organization?.Contact),
                                    organizationAddress,
                                    emails,
                                    phones);
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
                Organization.Contact = PersonHelper.CreateWriteDtoFromReadDto(organization?.Contact);

            return Organization;
        }
    }
}
