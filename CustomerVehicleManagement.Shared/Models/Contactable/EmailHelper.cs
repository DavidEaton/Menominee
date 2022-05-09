using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using Base = CustomerVehicleManagement.Domain.BaseClasses;

namespace CustomerVehicleManagement.Shared.Models.Contactable
{
    public class EmailHelper
    {
        public static string GetPrimaryEmail(Base.Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0
                ? entity.Emails.FirstOrDefault(email => email?.IsPrimary == true)?.Address
                : string.Empty;
        }

        public static string GetOrdinalEmail(Base.Contactable entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0
                ? entity.Emails[position]?.ToString()
                : string.Empty;
        }


        public static EmailToRead CreateEmail(Email email)
        {
            if (email != null)
            {
                return new EmailToRead()
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                };
            }

            return null;
        }

        public static IReadOnlyList<EmailToRead> CreateEmails(IEnumerable<Email> emails)
        {
            return emails
                .Select(email =>
                        CreateEmail(email))
                .ToList();
        }
    }
}
