using BaseClasses = CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api.Email
{
    public class EmailDtoHelper
    {
        public static IList<EmailReadDto> ToReadDto(IEnumerable<Domain.Entities.Email> emails)
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

            return emailReadDtos;
        }
        public static IList<Domain.Entities.Email> UpdateDtosToEntities(IList<EmailUpdateDto> emailUpdateDtos)
        {
            var emails = new List<Domain.Entities.Email>();

            if (emailUpdateDtos != null)
            {
                foreach (var email in emailUpdateDtos)
                    emails.Add(new Domain.Entities.Email(email.Address, email.IsPrimary));
            }

            return emails;
        }

        internal static string GetOrdinalEmail(BaseClasses.Contactable entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0 ? entity.Emails[position].ToString() : string.Empty;
        }

        internal static string GetPrimaryEmail(BaseClasses.Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0 ? entity.Emails.FirstOrDefault(email => email.IsPrimary == true).Address : string.Empty;
        }


    }
}
