using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailToEdit
    {
        public string Address { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;

        public static IList<Email> ConvertToEntities(IList<EmailToEdit> emails)
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