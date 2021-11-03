using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class PhoneHelper
    {
        public static string GetPrimaryPhone(Contactable entity)
        {
            if (entity == null || entity?.Phones == null || entity?.Phones?.Count < 1)
                return string.Empty;

            var number = entity.Phones.FirstOrDefault(phone => phone?.IsPrimary == true);

            if (number != null)
                return number.ToString();

            return string.Empty;
        }

        public static string GetPrimaryPhoneType(Contactable entity)
        {
            if (entity == null || entity?.Phones?.Count < 1)
                return string.Empty;

            var number = entity.Phones.FirstOrDefault(phone => phone?.IsPrimary == true);

            if (number != null)
                return number.PhoneType.ToString();

            return string.Empty;
        }

        public static string GetOrdinalPhone(Person person, int position)
        {
            if (person == null || person?.Phones == null || person?.Phones?.Count < 1)
                return string.Empty;

            return person?.Phones.Count > 0 ? person?.Phones[position].ToString() : string.Empty;
        }

        public static string GetOrdinalPhoneType(Person person, int position)
        {
            if (person == null || person?.Phones == null || person?.Phones?.Count < 1)
                return string.Empty;

            return person?.Phones.Count > 0 ? person?.Phones[position].PhoneType.ToString() : string.Empty;
        }
    }
}
