using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Base = CustomerVehicleManagement.Domain.BaseClasses;

namespace CustomerVehicleManagement.Shared.Models.Contactable
{
    public class PhoneHelper
    {
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


        private static PhoneToWrite ConvertReadToWriteDto(PhoneToRead phone)
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

        private static PhoneToRead ConvertToReadDto(Phone phone)
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

        public static IReadOnlyList<PhoneToWrite> ConvertToWriteDtos(IReadOnlyList<Phone> phones)
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

        public static string GetPrimaryPhone(Base.Contactable entity)
        {
            if (entity == null || entity?.Phones == null || entity?.Phones?.Count < 1)
                return string.Empty;

            return entity.Phones.Count > 0
                ? entity.Phones.FirstOrDefault(phone => phone?.IsPrimary == true)?.ToString()
                : string.Empty;
        }

        public static string GetPrimaryPhoneType(Base.Contactable entity)
        {
            if (entity == null || entity?.Phones?.Count < 1)
                return string.Empty;

            return entity.Phones.Count > 0
                ? entity.Phones.FirstOrDefault(phone => phone?.IsPrimary == true)?.PhoneType.ToString()
                : string.Empty;
        }

        public static string GetOrdinalPhone(Person person, int position)
        {
            if (person == null || person?.Phones == null || person?.Phones?.Count < 1)
                return string.Empty;

            return person?.Phones.Count >
                0 ? person?.Phones[position].ToString()
                : string.Empty;
        }

        public static string GetOrdinalPhoneType(Person person, int position)
        {
            if (person == null || person?.Phones == null || person?.Phones?.Count < 1)
                return string.Empty;

            return person?.Phones.Count > 0
                ? person?.Phones[position]?.PhoneType.ToString()
                : string.Empty;
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


    }
}