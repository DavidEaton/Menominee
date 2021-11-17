using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailToWrite
    {
        public string Address { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;

        public static Email ConvertToEntity(EmailToWrite email)
        {
            if (email != null)
            {
                return new Email(email.Address, email.IsPrimary);
            }

            return null;
        }

        public static IList<Email> ConvertToEntities(IList<EmailToWrite> emails)
        {
            var emailEntities = new List<Email>();

            if (emails != null)
            {
                foreach (var email in emails)
                    emailEntities.Add(new Email(email.Address, email.IsPrimary));
            }

            return emailEntities;
        }
    }
}