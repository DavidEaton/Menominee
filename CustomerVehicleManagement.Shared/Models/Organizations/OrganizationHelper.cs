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
        public static Organization ConvertWriteDtoToEntity(OrganizationToWrite organization)
        {
            if (organization is null)
                return null;

            Address organizationAddress = null;
            List<Phone> phones = new();
            List<Email> emails = new();
            OrganizationName organizationName;

            organizationName = OrganizationName.Create(organization.Name).Value;
            if (organization?.Address is not null)
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

            return Organization.Create(organizationName,
                                    organization.Note,
                                    PersonHelper.ConvertWriteDtoToEntity(organization?.Contact),
                                    organizationAddress,
                                    emails,
                                    phones).Value;
        }

        public static OrganizationToWrite CovertReadToWriteDto(OrganizationToRead organization)
        {
            return organization is not null
                ? new OrganizationToWrite()
                {
                    Name = organization.Name,
                    Note = organization?.Note,
                    Address = AddressHelper.ConvertReadToWriteDto(organization.Address),
                    Phones = PhoneHelper.ConvertReadToWriteDtos(organization.Phones),
                    Emails = EmailHelper.ConvertReadToWriteDto(organization.Emails),
                    Contact = PersonHelper.ConvertReadToWriteDto(organization?.Contact)
                }
                : null;
        }

        public static OrganizationToRead ConvertEntityToReadDto(Organization organization)
        {
            return organization is not null
                ? new OrganizationToRead()
                {
                    Id = organization.Id,
                    Name = organization.Name.Name,
                    Address = AddressHelper.ConvertEntityToReadDto(organization.Address),
                    Note = organization.Note,
                    Phones = PhoneHelper.ConvertEntitiesToReadDtos(organization.Phones),
                    Emails = EmailHelper.ConvertEntitiesToReadDtos(organization.Emails),
                    Contact = PersonHelper.ConvertToReadDto(organization.Contact)
                }
                : null;
        }

        public static OrganizationToReadInList ConvertEntityToReadInListDto(Organization organization)
        {
            return organization is not null
                ? new OrganizationToReadInList
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
                }
                : null;
        }
    }
}