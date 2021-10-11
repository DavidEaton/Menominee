using Menominee.Common;
using Menominee.Common.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Email : Entity
    {
        public static readonly string EmailEmptyMessage = "Email address cannot be empty";
        public const string EmailErrorMessage = "Email address and/or its format is invalid";

        public string Address { get; private set; }
        public bool IsPrimary { get; private set; }

        public Email(string address, bool isPrimary)
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

        public Email NewAddress(string newAddress)
        {
            return new Email(newAddress, IsPrimary);
        }

        public Email NewPrimary(bool newPrimary)
        {
            return new Email(Address, newPrimary);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null))
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
