using AutoMapper;
using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using Helper = CustomerVehicleManagement.Api.Utilities.ContactableHelpers;


namespace CustomerVehicleManagement.Api.Utilities
{
    public static class DtoHelpers
    {
        public static OrganizationsInListDto CreateOrganizationsListDtoFromDomain(Organization organization)
        {
            return new OrganizationsInListDto()
            {
                AddressLine = organization?.Address?.AddressLine,
                City = organization?.Address?.City,
                Id = organization.Id,
                Name = organization.Name,
                ContactName = organization?.Contact?.Name?.LastFirstMiddle,
                PostalCode = organization?.Address?.PostalCode,
                State = organization?.Address?.State,
                PrimaryPhone = Helper.GetPrimaryPhone(organization) ?? Helper.GetOrdinalPhone(organization, 0),
                PrimaryPhoneType = Helper.GetPrimaryPhoneType(organization) ?? Helper.GetOrdinalPhoneType(organization, 0),
                Notes = organization.Notes
            };
        }

        public static PersonInListDto CreatePersonsListDtoFromDomain(Person person)
        {
            return new PersonInListDto()
            {
                AddressLine = person?.Address?.AddressLine,
                City = person?.Address?.City,
                Id = person.Id,
                Name = person.Name.LastFirstMiddle,
                PostalCode = person?.Address?.PostalCode,
                State = person?.Address?.State,
                PrimaryPhone = Helper.GetPrimaryPhone(person) ?? Helper.GetOrdinalPhone(person, 0),
                PrimaryPhoneType = Helper.GetPrimaryPhoneType(person) ?? Helper.GetOrdinalPhoneType(person, 0),
            };
        }

        /// <summary>
        /// Map the PersonUpdateDto back to the domain entity
        /// REPLACE THIS METHOD WITH AUTOMAPPER
        /// </summary>
        /// <param name="personUpdateDto"></param>
        /// <param name="person"></param>
        public static void ConvertPersonUpdateDtoToDomainModel(
            PersonUpdateDto personUpdateDto,
            Person person,
            IMapper mapper)
        {
            person.SetName(personUpdateDto.Name);
            person.SetGender(personUpdateDto.Gender);
            person.SetAddress(personUpdateDto.Address);
            person.SetBirthday(personUpdateDto.Birthday);
            person.SetDriversLicense(personUpdateDto.DriversLicense);
            person.SetPhones(mapper.Map<IList<Phone>>(personUpdateDto.Phones));
            person.SetEmails(mapper.Map<IList<Email>>(personUpdateDto.Emails));
        }

        public static void ConvertOrganizationUpdateDtoToDomainModel(
            OrganizationUpdateDto organizationUpdateDto,
            Organization organizationFromRepository)
        {
            organizationFromRepository.SetName(organizationUpdateDto.Name);
            //organizationFromRepository.SetContact(organizationUpdateDto.Contact);
            organizationFromRepository.SetAddress(organizationUpdateDto.Address);
            organizationFromRepository.SetNotes(organizationUpdateDto.Notes);
            organizationFromRepository.SetPhones(organizationUpdateDto.Phones);
            organizationFromRepository.SetEmails(organizationUpdateDto.Emails);
        }

        public static OrganizationReadDto ConvertOrganizationDomainToReadDto(Organization organization)
        {
            return new OrganizationReadDto
            {
                Id = organization.Id,
                Name = organization.Name,
                Contact = (organization.Contact == null) ? null : new ContactReadDto
                {
                    Id = organization.Contact.Id,
                    Name = organization.Contact.Name.LastFirstMiddle,
                    Phones = organization.Contact?.Phones.Select(x => new PhoneReadDto
                    {
                        Number = x.Number,
                        PhoneType = x.PhoneType.ToString(),
                        IsPrimary = x.IsPrimary
                    })
                },
                AddressLine = organization?.Address?.AddressLine,
                City = organization?.Address?.City,
                State = organization?.Address?.State,
                PostalCode = organization?.Address?.PostalCode,
                Notes = organization?.Notes,
                //Phones = (organization.Phones == null) ? null : Helper.MapDomainPhoneToReadDto(organization?.Phones),
                //Emails = (organization.Emails == null) ? null : Helper.MapDomainEmailToReadDto(organization?.Emails),
                Phones = Helper.MapDomainPhoneToReadDto(organization?.Phones) ?? null,
                Emails = Helper.MapDomainEmailToReadDto(organization?.Emails) ?? null
            };
        }
    }
}
