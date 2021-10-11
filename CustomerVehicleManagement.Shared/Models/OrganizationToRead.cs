using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class OrganizationToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public AddressToRead Address { get; set; }
        public PersonToRead Contact { get; set; }
        public string Note { get; set; }
        public IReadOnlyList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IReadOnlyList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();

        public static OrganizationToRead ConvertToDto(Organization organization)
        {
            if (organization != null)
            {
                return new OrganizationToRead()
                {
                    Id = organization.Id,
                    Name = organization.Name.Name,
                    Address = AddressToRead.ConvertToDto(organization.Address),
                    Note = organization.Note,
                    Phones = PhoneToRead.ConvertToDto(organization.Phones),
                    Emails = EmailToRead.ConvertToDto(organization.Emails),
                    Contact = PersonToRead.ConvertToDto(organization.Contact)
                };
            }

            return null;
        }
    }
}
