using Menominee.Domain.Entities;
using Menominee.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.Contactable
{
    public class EmailHelper
    {
        public static Email ConvertWriteDtoToEntity(EmailToWrite email)
        {
            return email is not null
                ? Email.Create(email.Address, email.IsPrimary).Value
                : null;
        }

        public static string GetPrimaryEmail(ICustomerEntity entity)
        {
            switch (entity)
            {
                case Person person when person?.Phones != null && person.Phones.Any():
                    return person.Emails.FirstOrDefault(email => email?.IsPrimary == true)?.Address ?? null;

                case Business business when business?.Phones != null && business.Phones.Any():
                    return business.Emails.FirstOrDefault(email => email?.IsPrimary == true)?.Address ?? null;

                default:
                    return string.Empty;
            }
        }

        public static string GetOrdinalEmail(ICustomerEntity entity, int position)
        {
            if (entity is null)
                return string.Empty;

            switch (entity)
            {
                case Person person when person?.Emails is not null && person.Emails.Any():
                    return person.Emails.Count > 0
                    ? person.Emails[position]?.ToString()
                    : string.Empty;

                case Business business when business?.Emails is not null && business.Emails.Any():
                    return business.Emails.Count > 0
                    ? business.Emails[position]?.ToString()
                    : string.Empty;

                default:
                    return string.Empty;
            }
        }


        public static EmailToRead ConvertToReadDto(Email email)
        {
            if (email is not null)
            {
                return new EmailToRead()
                {
                    Id = email.Id,
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                };
            }

            return null;
        }

        public static List<EmailToWrite> ConvertReadToWriteDtos(IReadOnlyList<EmailToRead> emails)
        {
            return emails
                .Select(email =>
                        ConvertReadToWriteDto(email))
                .ToList();
        }

        public static EmailToWrite ConvertReadToWriteDto(EmailToRead email)
        {
            if (email is not null)
            {
                return new EmailToWrite()
                {
                    Id = email.Id,
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                };
            }

            return null;
        }

        public static List<EmailToRead> ConvertToReadDtos(IReadOnlyList<Email> emails)
        {
            return emails
                .Select(email =>
                        ConvertToReadDto(email))
                .ToList();
        }

        public static List<EmailToWrite> ConvertToWriteDtos(IReadOnlyList<Email> emails)
        {
            return emails
                .Select(email =>
                        ConvertToWriteDto(email))
                .ToList();
        }

        public static EmailToWrite ConvertToWriteDto(Email email)
        {
            return (email is not null)
                ? new EmailToWrite()
                {
                    Id = email.Id,
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                }
                : null;
        }

        internal static List<EmailToRead> ConvertWriteToReadDtos(List<EmailToWrite> emails)
        {
            return emails
                .Select(email =>
                    ConvertWriteToReadDto(email))
                .ToList();
        }

        private static EmailToRead ConvertWriteToReadDto(EmailToWrite email)
        {
            return (email is not null)
                ? new()
                {
                    Id = email.Id,
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                }
                : new();
        }
    }
}