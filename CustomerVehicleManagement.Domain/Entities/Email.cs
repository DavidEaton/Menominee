using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Email : Entity, IHasPrimary
    {
        public static readonly string InvalidMessage = "Email address and/or its format is invalid";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 254;
        public static readonly string MinimumLengthMessage = $"Email address cannot be less than {MinimumLength} character(s) in length.";
        public static readonly string MaximumLengthMessage = $"Email address cannot be over {MaximumLength} characters in length.";
        public static readonly string EmptyMessage = $"Email address cannot be empty.";

        public string Address { get; private set; }
        public bool IsPrimary { get; private set; }

        private Email(string address, bool isPrimary)
        {
            Address = address;
            IsPrimary = isPrimary;
        }

        public static Result<Email> Create(string address, bool isPrimary)
        {
            if (string.IsNullOrWhiteSpace(address))
                return Result.Failure<Email>(EmptyMessage);

            address = (address ?? string.Empty).Trim();

            if (address.Length < MinimumLength)
                return Result.Failure<Email>(MinimumLengthMessage);

            // https://www.rfc-editor.org/rfc/rfc5321#section-4.5.3
            if (address.Length > MaximumLength)
                return Result.Failure<Email>(MaximumLengthMessage);

            var emailAddressAttribute = new EmailAddressAttribute();

            if (!emailAddressAttribute.IsValid(address))
                return Result.Failure<Email>(InvalidMessage);

            return Result.Success(new Email(address, isPrimary));
        }

        public void SetAddress(string address)
        {

            address = address.Trim();

            var emailAddressAttribute = new EmailAddressAttribute();

            if (!emailAddressAttribute.IsValid(address))
                throw new ArgumentOutOfRangeException(nameof(address), InvalidMessage);

            Address = address;
        }

        public void SetIsPrimary(bool isPrimary)
        {
            IsPrimary = isPrimary;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Email() { }

        #endregion
    }
}
