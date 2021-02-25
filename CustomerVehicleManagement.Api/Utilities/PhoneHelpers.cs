using CustomerVehicleManagement.Domain.Interfaces;
using System.Linq;

namespace CustomerVehicleManagement.Api.Utilities
{
    public static class PhoneHelpers
    {

        public static string GetPrimaryPhone(IListOfPhone entity)
        {
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
    }
}
