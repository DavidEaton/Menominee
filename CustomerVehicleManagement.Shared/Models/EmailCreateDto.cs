using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
{
    public class EmailCreateDto
    {
        public static readonly string EmailEmptyMessage = "Email address cannot be empty";
        public const string EmailErrorMessage = "Email address and/or its format is invalid";

        public string Address { get; private set; }
        public bool IsPrimary { get; private set; }

        [JsonConstructor]
        public EmailCreateDto(string address, bool isPrimary)
        {
            try
            {
                Guard.ForNullOrEmpty(address, "address");

            }
            catch (Exception)
            {
                throw new ArgumentException(EmailEmptyMessage, nameof(address));
            }

            var emailAddressAttribute = new EmailAddressAttribute();

            if (!emailAddressAttribute.IsValid(address))
            {
                throw new ArgumentException(EmailErrorMessage);
            }

            Address = address;
            IsPrimary = isPrimary;
        }

        public static Email ConvertToEntity(EmailCreateDto email)
        {
            if (email != null)
            {
                return new Email(email.Address, email.IsPrimary);
            }

            return null;
        }
    }
}
