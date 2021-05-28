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
        /// TODO: REPLACE THIS METHOD WITH AUTOMAPPER
        /// </summary>
        /// <param name="personUpdateDto"></param>
        /// <param name="person"></param>
        /// <param name="mapper"></param>
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

        /// <summary>
        /// Map the OrganizationUpdateDto back to the domain entity
        /// TODO: REPLACE THIS METHOD WITH AUTOMAPPER
        /// </summary>
        /// <param name="organizationUpdateDto"></param>
        /// <param name="organization"></param>
        /// <param name="mapper"></param>
        public static void ConvertOrganizationUpdateDtoToDomainModel(
            OrganizationUpdateDto organizationUpdateDto,
            Organization organization,
            IMapper mapper)
        {
            var organizationNameOrError = OrganizationName.Create(organizationUpdateDto.Name);
            if (organizationNameOrError.IsFailure)
                return;

            organization.SetName(organizationNameOrError.Value);
            //organization.SetContact(organizationUpdateDto.Contact);
            organization.SetAddress(organizationUpdateDto.Address);
            organization.SetNotes(organizationUpdateDto.Notes);
            organization.SetPhones(mapper.Map<IList<Phone>>(organizationUpdateDto.Phones));
            organization.SetEmails(mapper.Map<IList<Email>>(organizationUpdateDto.Emails));
        }
    }
}
