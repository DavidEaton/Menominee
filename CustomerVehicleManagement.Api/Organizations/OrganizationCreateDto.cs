using CustomerVehicleManagement.Shared.Models;
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

        public OrganizationCreateDto(string name, string notes)
            : this(name, notes, null)
        {
        }

        public OrganizationCreateDto(string name, string notes, Address address)
            : this(name, notes, address, null)
        {
        }

        public OrganizationCreateDto(string name, string notes, Address address, PersonCreateDto contact)
            : this(name, notes, address, contact, null)
        {
        }

        public OrganizationCreateDto(string name, string notes, Address address, PersonCreateDto contact, IList<PhoneCreateDto> phones)
            : this(name, notes, address, contact, phones, null)
        {
        }

        [JsonConstructor]
        public OrganizationCreateDto(string name, string notes, Address address, PersonCreateDto contact, IList<PhoneCreateDto> phones, IList<EmailCreateDto> emails)
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
            if (!string.IsNullOrWhiteSpace(notes)) Notes = notes;
        }

        public IList<PhoneCreateDto> Phones { get; private set; } = new List<PhoneCreateDto>();
        public IList<EmailCreateDto> Emails { get; private set; } = new List<EmailCreateDto>();

        public string Name { get; private set; }

        public virtual PersonCreateDto Contact { get; private set; }

        public Address Address { get; private set; }

        public string Notes { get; private set; }
    }
}
