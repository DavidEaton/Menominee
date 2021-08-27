using CustomerVehicleManagement.Api.Email;
using CustomerVehicleManagement.Api.Phones;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Persons
{
    public class PersonDtoHelper
    {
        public static PersonReadDto ToReadDto(Person person)
        {
            PersonReadDto contactReadDto = new();

            contactReadDto.Id = person.Id;
            contactReadDto.Address = person?.Address;
            contactReadDto.Birthday = person?.Birthday;
            contactReadDto.DriversLicense = person?.DriversLicense;
            contactReadDto.Gender = person.Gender;
            contactReadDto.Name = person.Name.LastFirstMiddle;
            contactReadDto.Emails = (IReadOnlyList<EmailReadDto>)EmailDtoHelper.ToReadDto(person?.Emails);
            contactReadDto.Phones = (IReadOnlyList<PhoneReadDto>)PhonesDtoHelper.ToReadDto(person?.Phones);

            return contactReadDto;
        }

        /// <summary>
        /// Map the PersonUpdateDto back to the domain entity
        /// </summary>
        /// <param name="personUpdateDto"></param>
        /// <param name="person"></param>
        public static void PersonUpdateDtoToEntity(PersonUpdateDto personUpdateDto,
                                                   Person person)
        {
            person.SetName(personUpdateDto.Name);
            person.SetGender(personUpdateDto.Gender);

            person.SetAddress(personUpdateDto?.Address);
            person.SetBirthday(personUpdateDto?.Birthday);
            person.SetDriversLicense(personUpdateDto?.DriversLicense);
            person.SetPhones(PhonesDtoHelper.UpdateDtosToEntities(personUpdateDto?.Phones));
            person.SetEmails(EmailDtoHelper.UpdateDtosToEntities(personUpdateDto?.Emails));
        }

        public static PersonInListDto ToPersonInListDto(Person person)
        {
            return new PersonInListDto()
            {
                AddressLine = person?.Address?.AddressLine,
                Birthday = person?.Birthday,
                City = person?.Address?.City,
                Id = person.Id,
                Name = person.Name.LastFirstMiddle,
                PostalCode = person?.Address?.PostalCode,
                State = person?.Address?.State.ToString(),
                PrimaryPhone = PhonesDtoHelper.GetPrimaryPhone(person) ?? PhonesDtoHelper.GetOrdinalPhone(person, 0),
                PrimaryPhoneType = PhonesDtoHelper.GetPrimaryPhoneType(person) ?? PhonesDtoHelper.GetOrdinalPhoneType(person, 0),
                PrimaryEmail = EmailDtoHelper.GetPrimaryEmail(person) ?? EmailDtoHelper.GetOrdinalEmail(person, 0)
            };
        }
    }
}
