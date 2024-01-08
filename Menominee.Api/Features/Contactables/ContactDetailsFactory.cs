using CSharpFunctionalExtensions;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Domain.ValueObjects;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Api.Features.Contactables
{
    public static class ContactDetailsFactory
    {
        public static Result<ContactDetails> Create(
            IReadOnlyList<PhoneToWrite> phonesToWrite,
            IReadOnlyList<EmailToWrite> emailsToWrite,
            AddressToWrite addressToWrite)
        {
            var phones = (phonesToWrite ?? new List<PhoneToWrite>())
                .Select(phone =>
                {
                    var createdPhone = Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value;
                    typeof(Entity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty).SetValue(createdPhone, phone.Id);
                    return createdPhone;
                })
                .ToArray();

            var emails = (emailsToWrite ?? new List<EmailToWrite>())
                .Select(email =>
                {
                    var createdEmail = Email.Create(email.Address, email.IsPrimary).Value;
                    typeof(Entity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty).SetValue(createdEmail, email.Id);
                    return createdEmail;
                })
                .ToArray();

            var maybeAddress = Maybe<Address>.None;
            if (addressToWrite is not null && addressToWrite.IsNotEmpty)
            {
                var addressResult = Address.Create(
                    addressToWrite.AddressLine1,
                    addressToWrite.City,
                    addressToWrite.State,
                    addressToWrite.PostalCode,
                    addressToWrite.AddressLine2);

                if (addressResult.IsFailure)
                {
                    // TODO: the case where the address creation fails but it's not critical:
                    // log it and continue
                }
                else
                {
                    maybeAddress = Maybe<Address>.From(addressResult.Value);
                }
            }

            return ContactDetails.Create(phones, emails, maybeAddress);
        }
    }
}
