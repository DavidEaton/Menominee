using SharedKernel.Utilities;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
{
    public class OrganizationCreateDto
    {
        public static readonly string OrganizationNameEmptyMessage = "Name cannot be empty";

        public OrganizationCreateDto(string name)
            : this(name, null)
        {
        }

        public OrganizationCreateDto(string name, string note)
            : this(name, note, null)
        {
        }

        public OrganizationCreateDto(string name, string note, Address address)
            : this(name, note, address, null)
        {
        }

        public OrganizationCreateDto(string name, string note, Address address, PersonCreateDto contact)
            : this(name, note, address, contact, null)
        {
        }

        public OrganizationCreateDto(string name, string note, Address address, PersonCreateDto contact, IList<PhoneCreateDto> phones)
            : this(name, note, address, contact, phones, null)
        {
        }

        [JsonConstructor]
        public OrganizationCreateDto(string name, string note, Address address, PersonCreateDto contact, IList<PhoneCreateDto> phones, IList<EmailCreateDto> emails)
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
            if (!string.IsNullOrWhiteSpace(note)) Note = note;
        }

        public IList<PhoneCreateDto> Phones { get; private set; } = new List<PhoneCreateDto>();
        public IList<EmailCreateDto> Emails { get; private set; } = new List<EmailCreateDto>();

        public string Name { get; private set; }

        public virtual PersonCreateDto Contact { get; private set; }

        public Address Address { get; private set; }

        public string Note { get; private set; }
    }
}
