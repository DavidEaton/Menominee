using CustomerVehicleManagement.Domain.BaseClasses;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class EmailHelper
    {
        public static string GetPrimaryEmail(Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0 ? entity.Emails.FirstOrDefault(email => email.IsPrimary == true).Address : string.Empty;
        }

        public static string GetOrdinalEmail(Contactable entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0 ? entity.Emails[position].ToString() : string.Empty;
        }
    }
}
