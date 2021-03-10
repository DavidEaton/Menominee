using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Utilities;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Data.Models
{
    public class OrganizationCreateDto : Contactable
    {
        public static readonly string OrganizationNameEmptyMessage = "Name cannot be empty";

        public OrganizationCreateDto(string name)
            : this(name, null)
        {
        }

        public OrganizationCreateDto(string name, Address address)
            : this(name, address, null)
        {
        }

        public OrganizationCreateDto(string name, Address address, PersonCreateDto contact)
            : this(name, address, contact, null)
        {
        }

        public OrganizationCreateDto(string name, Address address, PersonCreateDto contact, IList<PhoneCreateDto> phones)
            : this(name, address, contact, phones, null)
        {
        }

        [JsonConstructor]
        public OrganizationCreateDto(string name, Address address, PersonCreateDto contact, IList<PhoneCreateDto> phones, IList<EmailCreateDto> emails)
        {
            try
            {
                Guard.ForNullOrEmpty(name, "name");

            }
            catch (Exception)
            {
                throw new ArgumentException(OrganizationNameEmptyMessage, nameof(name));
            }

            Name = name;
            if (address != null) Address = address;
            if (contact != null) Contact = contact;
            if (phones != null) SetPhones(ConvertCreateDtosToPhones(phones));
            if (emails != null) SetEmails(ConvertCreateDtosToEmails(emails));
        }

        private IList<Email> ConvertCreateDtosToEmails(IList<EmailCreateDto> emails)
        {
            IList<Email> newPhones = new List<Email>();

            foreach (var email in emails)
                newPhones.Add(new Email(email.Address, email.IsPrimary));

            return newPhones;
        }

        private IList<Phone> ConvertCreateDtosToPhones(IList<PhoneCreateDto> phones)
        {
            IList<Phone> newPhones = new List<Phone>();

            foreach (var phone in phones)
                newPhones.Add(new Phone(phone.Number, phone.PhoneType, phone.Primary));

            return newPhones;
        }

        public string Name { get; private set; }
        public virtual PersonCreateDto Contact { get; private set; }
        public Address Address { get; private set; }
        public string Notes { get; private set; }
    }
}
