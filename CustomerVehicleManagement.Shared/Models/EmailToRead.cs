using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailToRead
    {
        public string Address { get; set; }
        public bool IsPrimary { get; set; }

        public static EmailToRead ConvertToDto(Email email)
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

        public static IReadOnlyList<EmailToRead> ConvertToDto(IEnumerable<Email> emails)
        {
            var emailReadDtos = new List<EmailToRead>();

            foreach (var email in emails)
            {
                emailReadDtos.Add(new EmailToRead
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                });
            }

            return emails
                .Select(email =>
                        ConvertToDto(email))
                .ToList();
        }
    }
}
