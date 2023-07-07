using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using Menominee.Common.ValueObjects;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Api.Common
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

            var address = Address.Create(
                addressToWrite.AddressLine,
                addressToWrite.City,
                addressToWrite.State,
                addressToWrite.PostalCode).Value;

            return ContactDetails.Create(phones, emails, address);
        }
    }
}
