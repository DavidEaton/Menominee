using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using Helper = CustomerVehicleManagement.Api.Utilities.ContactableHelpers;


namespace CustomerVehicleManagement.Api.Utilities
{
    public static class DtoHelpers
    {
        public static PersonInListDto PersonToPersonInListDto(Person person)
        {
            return new PersonInListDto()
            {
                AddressLine = person?.Address?.AddressLine,
                Birthday = person?.Birthday,
                City = person?.Address?.City,
                Id = person.Id,
                Name = person.Name.LastFirstMiddle,
                PostalCode = person?.Address?.PostalCode,
                State = person?.Address?.State,
                PrimaryPhone = Helper.GetPrimaryPhone(person) ?? Helper.GetOrdinalPhone(person, 0),
                PrimaryPhoneType = Helper.GetPrimaryPhoneType(person) ?? Helper.GetOrdinalPhoneType(person, 0),
            };
        }

        public static IList<Phone> PhonesUpdateDtoToPhones(IList<PhoneUpdateDto> phoneUpdateDtos)
        {
            var phones = new List<Phone>();

            if (phoneUpdateDtos != null)
            {
                foreach (var phone in phoneUpdateDtos)
                    phones.Add(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));
            }

            return phones;
        }

        public static IList<Email> EmailsUpdateDtoToEmails(IList<EmailUpdateDto> emailUpdateDtos)
        {
            var emails = new List<Email>();

            if (emailUpdateDtos != null)
            {
                foreach (var email in emailUpdateDtos)
                    emails.Add(new Email(email.Address, email.IsPrimary));
            }

            return emails;
        }

        /// <summary>
        /// Map the PersonUpdateDto back to the domain entity
        /// </summary>
        /// <param name="personUpdateDto"></param>
        /// <param name="person"></param>
        public static void PersonUpdateDtoToPerson(PersonUpdateDto personUpdateDto,
                                                               Person person)
        {
            person.SetName(personUpdateDto.Name);
            person.SetGender(personUpdateDto.Gender);
            person.SetAddress(personUpdateDto.Address);
            person.SetBirthday(personUpdateDto.Birthday);
            person.SetDriversLicense(personUpdateDto.DriversLicense);
            person.SetPhones(PhonesUpdateDtoToPhones(personUpdateDto.Phones));
            person.SetEmails(EmailsUpdateDtoToEmails(personUpdateDto.Emails));
        }
    }
}
