using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailUpdateDto
    {
        public string Address { get; set; }
        public bool IsPrimary { get; set; }

        public static IList<Email> ConvertToEntities(IList<EmailUpdateDto> emails)
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
