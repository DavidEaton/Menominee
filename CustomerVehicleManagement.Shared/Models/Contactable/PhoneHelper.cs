using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Base = CustomerVehicleManagement.Domain.BaseClasses;

namespace CustomerVehicleManagement.Shared.Models.Contactable
{
    public class PhoneHelper
    {
        public static IList<PhoneToRead> ConvertEntitiesToReadDtos(IList<Phone> phones)
        {
            return phones
                .Select(phone =>
                        ConvertEntityToReadDto(phone))
                .ToList();
        }

        public static IList<PhoneToWrite> CovertReadToWriteDtos(IList<PhoneToRead> phones)
        {
            return phones
                .Select(phone =>
                        CovertReadToWriteDto(phone))
                .ToList();
        }


        private static PhoneToWrite CovertReadToWriteDto(PhoneToRead phone)
        {
            return (phone is not null)
                ? new PhoneToWrite()
                    {
                        Number = phone.Number,
                        PhoneType = (PhoneType)Enum.Parse(typeof(PhoneType), phone.PhoneType),
                        IsPrimary = phone.IsPrimary
                    }
                : null;
        }

        private static PhoneToRead ConvertEntityToReadDto(Phone phone)
        {
            return (phone is not null)
                ? new PhoneToRead()
                    {
                        Id = phone.Id,
                        Number = phone.Number,
                        PhoneType = phone.PhoneType.ToString(),
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

        //    public static Phone UpdatePhone(Base.Contactable entity, PhoneToWrite phone)
        //    {
        //        var phoneToUpdate = entity.Phones.FirstOrDefault(basePhone => basePhone.Id == phone.Id);

        //        phoneToUpdate.SetNumber(phone.Number);
        //        phoneToUpdate.SetPhoneType(phone.PhoneType);
        //        phoneToUpdate.SetIsPrimary(phone.IsPrimary);
        //        phoneToUpdate.SetTrackingState(TrackingState.Modified);

        //        return phoneToUpdate;
        //    }
    }
}