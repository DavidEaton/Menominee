using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api.Utilities
{
    public static class ContactableHelpers
    {
        public static string GetPrimaryPhone(IContactLists entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).ToString() : string.Empty;
        }

        public static string GetPrimaryPhoneType(IContactLists entity)
        {
            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).PhoneType.ToString() : string.Empty;
        }

        public static string GetOrdinalPhoneType(IContactLists entity, int position)
        {
            return entity.Phones.Count > 0 ? entity.Phones[position].PhoneType.ToString() : string.Empty;
        }

        public static string GetOrdinalPhone(IContactLists entity, int position)
        {
            return entity.Phones.Count > 0 ? entity.Phones[position].ToString() : string.Empty;
        }

        public static IList<EmailReadDto> MapDomainEmailToReadDto(IEnumerable<Email> emails)
        {
            var emailReadDtos = new List<EmailReadDto>();

            foreach (var email in emails)
            {
                emailReadDtos.Add(new EmailReadDto
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                });
            }

            return emailReadDtos;
        }
        public static IEnumerable<EmailReadDto> MapEmailReadDtoToReadDto(IEnumerable<EmailReadDto> emails)
        {
            var emailReadDtos = new List<EmailReadDto>();

            foreach (var email in emails)
            {
                emailReadDtos.Add(new EmailReadDto
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                });
            }

            return emailReadDtos;
        }

        public static IList<PhoneReadDto> MapDomainPhoneToReadDto(IEnumerable<Phone> phones)
        {
            var phoneReadDtos = new List<PhoneReadDto>();

            foreach (var phone in phones)
            {
                phoneReadDtos.Add(new PhoneReadDto
                {
                    Number = phone.Number,
                    IsPrimary = phone.IsPrimary,
                    PhoneType = phone.PhoneType.ToString()
                });
            }

            return phoneReadDtos;
        }

        public static List<Phone> CreatePhones(PersonCreateDto personToCreate)
        {
            var phones = new List<Phone>();
            Phone newPhone;

            foreach (var phone in personToCreate.Phones)
            {
                newPhone = new Phone(phone.Number, phone.PhoneType, phone.IsPrimary);
                phones.Add(newPhone);
            }

            return phones;
        }


    }
}
