using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class PhoneHelper
    {
        public static string GetPrimaryPhone(Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone?.IsPrimary == true)?.ToString() : string.Empty;
        }

        public static string GetPrimaryPhoneType(Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone?.IsPrimary == true)?.PhoneType.ToString() : string.Empty;
        }

        public static string GetOrdinalPhone(Person person, int position)
        {
            if (person == null)
                return string.Empty;

            return person?.Phones.Count > 0 ? person?.Phones[position].ToString() : string.Empty;
        }

        public static string GetOrdinalPhoneType(Person person, int position)
        {
            if (person == null)
                return string.Empty;

            return person?.Phones.Count > 0 ? person?.Phones[position]?.PhoneType.ToString() : string.Empty;
        }
    }
}
