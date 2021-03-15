using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api.Utilities
{
    public static class ContactableHelpers
    {
        public static string GetPrimaryPhone(IListOfPhone entity)
        {
            if (entity == null)
                return null;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).ToString() : null;
        }

        public static string GetPrimaryPhoneType(IListOfPhone entity)
        {
            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).PhoneType.ToString() : null;
        }

        public static string GetOrdinalPhoneType(IListOfPhone entity, int position)
        {
            return entity.Phones.Count > 0 ? entity.Phones[position].PhoneType.ToString() : null;
        }

        public static string GetOrdinalPhone(IListOfPhone entity, int position)
        {
            return entity.Phones.Count > 0 ? entity.Phones[position].ToString() : null;
        }

        public static IEnumerable<EmailReadDto> MapDomainEmailToReadDto(IList<Email> emails)
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
                newPhone = new Phone(phone.Number, phone.PhoneType, phone.Primary);
                phones.Add(newPhone);
            }

            return phones;
        }


    }
}
