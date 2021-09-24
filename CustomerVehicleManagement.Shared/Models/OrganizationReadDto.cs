using CustomerVehicleManagement.Domain.Entities;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class OrganizationReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public AddressReadDto Address { get; set; }
        public PersonReadDto Contact { get; set; }
        public string Note { get; set; }
        public IReadOnlyList<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
        public IReadOnlyList<EmailReadDto> Emails { get; set; } = new List<EmailReadDto>();

        public static OrganizationReadDto ConvertToDto(Organization organization)
        {
            if (organization != null)
            {
                return new OrganizationReadDto()
                {
                    Id = organization.Id,
                    Name = organization.Name.Name,
                    Address = AddressReadDto.ConvertToDto(organization.Address),
                    Note = organization.Note,
                    Phones = PhoneReadDto.ConvertToDto(organization.Phones),
                    Emails = EmailReadDto.ConvertToDto(organization.Emails),
                    Contact = PersonReadDto.ConvertToDto(organization.Contact)
                };
            }

            return null;
        }
    }
}
