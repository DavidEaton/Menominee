using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Helpers;

namespace CustomerVehicleManagement.Shared.Models
{
    public class OrganizationToReadInList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => string.IsNullOrWhiteSpace(AddressLine) ? $"{string.Empty}" : $"{AddressLine} {City}, {State} {PostalCode}"; }
        public string PrimaryPhone { get; set; }
        public string PrimaryPhoneType { get; set; }
        public string PrimaryEmail { get; set; }
        public string Note { get; set; }
        public string ContactName { get; set; }
        public string ContactPrimaryPhone { get; set; }
        public static OrganizationToReadInList ConvertToDto(Organization organization)
        {
            if (organization != null)
            {
                return new OrganizationToReadInList
                {
                    Id = organization.Id,
                    Name = organization.Name.Name,
                    ContactName = organization?.Contact?.Name.LastFirstMiddle,
                    ContactPrimaryPhone = PhoneHelper.GetPrimaryPhone(organization?.Contact),

                    AddressLine = organization?.Address?.AddressLine,
                    City = organization?.Address?.City,
                    State = organization?.Address?.State.ToString(),
                    PostalCode = organization?.Address?.PostalCode,

                    Note = organization.Note,
                    PrimaryPhone = PhoneHelper.GetPrimaryPhone(organization),
                    PrimaryPhoneType = PhoneHelper.GetPrimaryPhoneType(organization),
                    PrimaryEmail = EmailHelper.GetPrimaryEmail(organization)
                };
            }

            return null;
        }
    }
}
