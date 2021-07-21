using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Api.Utilities
{
    public static class ContactableHelpers
    {
        public static string GetPrimaryPhone(Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).ToString() : string.Empty;
        }

        public static string GetPrimaryPhoneType(Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones.FirstOrDefault(phone => phone.IsPrimary == true).PhoneType.ToString() : string.Empty;
        }

        public static string GetOrdinalPhoneType(Contactable entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Phones.Count > 0 ? entity.Phones[position].PhoneType.ToString() : string.Empty;
        }

        internal static string GetOrdinalEmail(Contactable entity, int position)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0 ? entity.Emails[position].ToString() : string.Empty;
        }

        internal static string GetPrimaryEmail(Contactable entity)
        {
            if (entity == null)
                return string.Empty;

            return entity.Emails.Count > 0 ? entity.Emails.FirstOrDefault(email => email.IsPrimary == true).Address : string.Empty;
        }

        public static string GetOrdinalPhone(Contactable entity, int position)
        {
            return entity.Phones.Count > 0 ? entity.Phones[position].ToString() : string.Empty;
        }

        public static IList<EmailReadDto> MapDomainEmailToReadDto(IEnumerable<Email> emails)
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
    }
}
