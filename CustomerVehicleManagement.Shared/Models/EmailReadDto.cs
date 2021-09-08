using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailReadDto
    {
        public string Address { get; set; }
        public bool IsPrimary { get; set; }

        public static EmailReadDto ConvertToDto(Email email)
        {
            if (email != null)
            {
                return new EmailReadDto()
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                };
            }

            return null;
        }

        public static IReadOnlyList<EmailReadDto> ConvertToDto(IEnumerable<Email> emails)
        {
            var emailReadDtos = new List<EmailReadDto>();

            foreach (var email in emails)
            {
                emailReadDtos.Add(new EmailReadDto
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                });
            }

            return emails.Select(email => EmailReadDto.ConvertToDto(email)).ToList();
        }
    }
}
