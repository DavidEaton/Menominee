using CSharpFunctionalExtensions;
using System.ComponentModel.DataAnnotations;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Email : Menominee.Common.Entity
    {
        public static readonly string InvalidMessage = "Email address and/or its format is invalid";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 254;
        public static readonly string MinimumLengthMessage = $"Email address cannot be less than {MinimumLength} character(s) in length.";
        public static readonly string MaximumLengthMessage = $"Email address cannot be over {MaximumLength} characters in length.";
        public static readonly string EmptyMessage = $"Email address cannot be empty.";

        public string Address { get; }
        public bool IsPrimary { get; }

        private Email(string address, bool isPrimary)
        {
            Address = address;
            IsPrimary = isPrimary;
        }

        public static Result<Email> Create(string address, bool isPrimary)
        {
            if (string.IsNullOrWhiteSpace(address))
                return Result.Failure<Email>(EmptyMessage);

            address = (address ?? "").Trim();

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

        public Email NewAddress(string newAddress)
        {
            return Create(newAddress, IsPrimary).Value;
        }

        public Email NewPrimary(bool newPrimary)
        {
            return new Email(Address, newPrimary);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Menominee.Common.Entity;

            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (((Email)obj).Address == Address)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }

        #region ORM

        // EF requires an empty constructor
        protected Email() { }

        #endregion
    }
}
