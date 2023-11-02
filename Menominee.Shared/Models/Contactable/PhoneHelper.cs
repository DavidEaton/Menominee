using Menominee.Domain.Entities;
using Menominee.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Menominee.Shared.Models.Contactable
{
    public class PhoneHelper
    {
        public static Phone ConvertWriteDtoToEntity(PhoneToWrite phone)
        {
            return phone is not null
                ? Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value
                : null;
        }

        public static List<PhoneToRead> ConvertToReadDtos(IReadOnlyList<Phone> phones)
        {
            return phones
                .Select(phone =>
                        ConvertToReadDto(phone))
                .ToList();
        }

        public static List<PhoneToWrite> ConvertReadToWriteDtos(IReadOnlyList<PhoneToRead> phones)
        {
            return phones
                .Select(phone =>
                        ConvertReadToWriteDto(phone))
                .ToList();
        }


        public static PhoneToWrite ConvertReadToWriteDto(PhoneToRead phone)
        {
            return (phone is not null)
                ? new PhoneToWrite()
                {
                    Id = phone.Id,
                    Number = phone.Number,
                    PhoneType = phone.PhoneType,
                    IsPrimary = phone.IsPrimary
                }
                : null;
        }

        public static PhoneToRead ConvertToReadDto(Phone phone)
        {
            return (phone is not null)
                ? new PhoneToRead()
                {
                    Id = phone.Id,
                    PhoneType = phone.PhoneType,
                    Number = phone.Number,
                    IsPrimary = phone.IsPrimary
                }
                : null;
        }

        public static List<PhoneToWrite> ConvertToWriteDtos(IReadOnlyList<Phone> phones)
        {
            return phones
                .Select(phone =>
                        ConvertToWriteDto(phone))
                .ToList();
        }

        public static PhoneToWrite ConvertToWriteDto(Phone phone)
        {
            return (phone is not null)
                ? new PhoneToWrite()
                {
                    Id = phone.Id,
                    PhoneType = phone.PhoneType,
                    Number = phone.Number,
                    IsPrimary = phone.IsPrimary
                }
                : null;
        }

        public static Phone GetPrimaryPhone(ICustomerEntity entity)
        {
            switch (entity)
            {
                case Person person when person?.Phones != null && person.Phones.Any():
                    return person.Phones.FirstOrDefault(phone => phone?.IsPrimary == true) ?? null;

                case Business business when business?.Phones != null && business.Phones.Any():
                    return business.Phones.FirstOrDefault(phone => phone?.IsPrimary == true) ?? null;

                default:
                    return null;
            }
        }

        public static Phone GetOrdinalPhone(ICustomerEntity entity, int position)
        {
            switch (entity)
            {
                case Person person when person?.Phones is not null && person.Phones.Any():
                    return person?.Phones[position];

                case Business business when business?.Phones is not null && business.Phones.Any():
                    return business?.Phones[position];

                default:
                    return null;
            }
        }

        public static string FormatPhoneNumber(string number)
        {
            number = RemoveNonNumericCharacters(number);

            return number.Length switch
            {
                7 => Regex.Replace(number, @"(\d{3})(\d{4})", "$1-$2"),
                10 => Regex.Replace(number, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3"),
                _ => number,
            };
        }

        private static string RemoveNonNumericCharacters(string input)
        {
            return new string(input.Where(character => char.IsDigit(character)).ToArray());
        }

        internal static List<PhoneToRead> ConvertWriteToReadDtos(List<PhoneToWrite> phones)
        {
            return phones
                .Select(phone =>
                    ConvertWriteToReadDto(phone))
                .ToList();
        }

        private static PhoneToRead ConvertWriteToReadDto(PhoneToWrite phone)
        {
            return phone is not null
                ? new()
                {
                    Id = phone.Id,
                    Number = phone.Number,
                    PhoneType = phone.PhoneType,
                    IsPrimary = phone.IsPrimary
                }
                : new();
        }
    }
}