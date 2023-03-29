using System.Collections.Generic;
using System.Linq;
using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Api.Common
{
    public static class ContactDetailsFactory
    {
        // VK: the assumption is that all data is validated beforehand and if not, we just throw an exception
        public static ContactDetails Create(
            IList<PhoneToWrite> phonesToWrite,
            IList<EmailToWrite> emailsToWrite,
            AddressToWrite addressToWrite)
        {
            Phone[] phones = (phonesToWrite ?? new List<PhoneToWrite>())
                .Select(phone => Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value)
                .ToArray();

            Email[] emails = (emailsToWrite ?? new List<EmailToWrite>())
                .Select(email => Email.Create(email.Address, email.IsPrimary).Value)
                .ToArray();

            Address address = Address.Create(
                addressToWrite.AddressLine,
                addressToWrite.City,
                addressToWrite.State,
                addressToWrite.PostalCode).Value;

            return new ContactDetails(phones, emails, address);
        }
    }
}
