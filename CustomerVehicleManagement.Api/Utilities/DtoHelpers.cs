using AutoMapper;
using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.ValueObjects;
using System.Collections.Generic;
using Helper = CustomerVehicleManagement.Api.Utilities.ContactableHelpers;


namespace CustomerVehicleManagement.Api.Utilities
{
    public static class DtoHelpers
    {
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
            var organizationNameOrError = OrganizationName.Create(organizationUpdateDto.Name);
            if (organizationNameOrError.IsFailure)
                return;

            organizationFromRepository.SetName(organizationNameOrError.Value);
            //organizationFromRepository.SetContact(organizationUpdateDto.Contact);
            organizationFromRepository.SetAddress(organizationUpdateDto.Address);
            organizationFromRepository.SetNotes(organizationUpdateDto.Notes);
            organizationFromRepository.SetPhones(organizationUpdateDto.Phones);
            organizationFromRepository.SetEmails(organizationUpdateDto.Emails);
        }
    }
}
