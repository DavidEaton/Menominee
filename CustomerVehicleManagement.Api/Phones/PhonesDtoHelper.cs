using BaseClasses = CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api.Phones
{
    public class PhonesDtoHelper
    {
        public static IList<PhoneReadDto> ToReadDto(IList<Phone> phones)
        {
            List<PhoneReadDto> phoneReadDtos = new();

            if (phones != null)
                foreach (var phone in phones)
                    phoneReadDtos.Add(new PhoneReadDto() { Number = phone.Number, PhoneType = phone.PhoneType.ToString(), IsPrimary = phone.IsPrimary });

            return phoneReadDtos;
        }

        public static IList<Phone> UpdateDtosToEntities(IList<PhoneUpdateDto> phoneUpdateDtos)
        {
            var phones = new List<Phone>();

            if (phoneUpdateDtos != null)
            {
                foreach (var phone in phoneUpdateDtos)
                    phones.Add(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));
            }

            return phones;
        }

        public static string GetPrimaryPhone(BaseClasses.Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).ToString() : string.Empty;
        }

        public static string GetPrimaryPhoneType(BaseClasses.Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).PhoneType.ToString() : string.Empty;
        }

        public static string GetOrdinalPhoneType(BaseClasses.Contactable entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones[position].PhoneType.ToString() : string.Empty;
        }

        public static string GetOrdinalPhone(BaseClasses.Contactable entity, int position)
        {
            return entity.Phones.Count > 0 ? entity.Phones[position].ToString() : string.Empty;
        }
    }
}
