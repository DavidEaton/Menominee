using CustomerVehicleManagement.Api.Data.Models;
using CustomerVehicleManagement.Domain.Entities;
using Helper = CustomerVehicleManagement.Api.Utilities.IListOfPhoneHelpers;


namespace CustomerVehicleManagement.Api.Utilities
{
    public static class DtoHelpers
    {
        public static OrganizationsListDto CreateOrganizationsListDtoFromDomain(Organization organization)
        {
            return new OrganizationsListDto()
            {
                AddressLine = organization?.Address?.AddressLine,
                City = organization?.Address?.City,
                Id = organization.Id,
                Name = organization.Name,
                ContactName = organization?.Contact?.Name?.LastFirstMiddle,
                PostalCode = organization?.Address?.PostalCode,
                State = organization?.Address?.State,
                Phone = Helper.GetPrimaryPhone(organization) ?? Helper.GetOrdinalPhone(organization, 0),
                PhoneType = Helper.GetPrimaryPhoneType(organization) ?? Helper.GetOrdinalPhoneType(organization, 0),
                Notes = organization.Notes
            };
        }

        public static PersonListDto CreatePersonsListDtoFromDomain(Person person)
        {
            return new PersonListDto()
            {
                AddressLine = person?.Address?.AddressLine,
                City = person?.Address?.City,
                Id = person.Id,
                Name = person.Name.LastFirstMiddle,
                PostalCode = person?.Address?.PostalCode,
                State = person?.Address?.State,
                Phone = Helper.GetPrimaryPhone(person) ?? Helper.GetOrdinalPhone(person, 0),
                PhoneType = Helper.GetPrimaryPhoneType(person) ?? Helper.GetOrdinalPhoneType(person, 0),
            };
        }
    }
}
