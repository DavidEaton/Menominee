using CustomerVehicleManagement.Domain.Interfaces;
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
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).PhoneType.ToString() : string.Empty;
        }

        internal static string GetOrdinalPhoneType(IContactLists entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones[position].PhoneType.ToString() : string.Empty;
        }

        internal static string GetOrdinalEmail(IContactLists entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0 ? entity.Emails[position].ToString() : string.Empty;
        }

        internal static string GetPrimaryEmail(IContactLists entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0 ? entity.Emails.FirstOrDefault(email => email.IsPrimary == true).Address : string.Empty;
        }

        internal static string GetOrdinalPhone(IContactLists entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones[position].ToString() : string.Empty;
        }
    }
}
