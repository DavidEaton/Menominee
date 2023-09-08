using CSharpFunctionalExtensions;
using Menominee.Common.ValueObjects;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Entity = Menominee.Common.Entity;

namespace Menominee.Api.Common
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

            var address = addressToWrite is not null
                ? Address.Create(
                    addressToWrite.AddressLine1,
                    addressToWrite.City,
                    addressToWrite.State,
                    addressToWrite.PostalCode,
                    addressToWrite.AddressLine2).Value
                : null;

            return ContactDetails.Create(phones, emails, address);
        }
    }
}
