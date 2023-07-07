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

        public static List<EmailToWrite> ConvertReadToWriteDto(IReadOnlyList<EmailToRead> emails)
        {
            return emails
                .Select(email =>
                        ConvertReadToWriteDto(email))
                .ToList();
        }

        private static EmailToWrite ConvertReadToWriteDto(EmailToRead email)
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

        public static IReadOnlyList<EmailToWrite> ConvertToWriteDtos(IReadOnlyList<Email> emails)
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
    }
}