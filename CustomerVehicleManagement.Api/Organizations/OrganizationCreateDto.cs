using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using SharedKernel.Utilities;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationCreateDto
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
            if (phones != null) Phones = phones;
            if (emails != null) Emails = emails;
        }

        public IList<PhoneCreateDto> Phones { get; private set; } = new List<PhoneCreateDto>();
        public IList<EmailCreateDto> Emails { get; private set; } = new List<EmailCreateDto>();

        public string Name { get; private set; }

        public virtual PersonCreateDto Contact { get; private set; }

        public Address Address { get; private set; }

        public string Notes { get; private set; }
    }
}
