using CustomerVehicleManagement.Api.Email;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationDtoHelper
    {
        public static OrganizationReadDto ToReadDto(Organization organization)
        {
            OrganizationReadDto organizationReadDto = new();

            organizationReadDto.Id = organization.Id;
            organizationReadDto.Address = organization.Address;
            organizationReadDto.Name = organization.Name.Name;
            organizationReadDto.Note = organization.Note;
            organizationReadDto.Emails = EmailDtoHelper.ToReadDto(organization.Emails);
            organizationReadDto.Phones = PhonesDtoHelper.ToReadDto(organization.Phones);

            if (organization.Contact != null)
                organizationReadDto.Contact = PersonDtoHelper.ToReadDto(organization.Contact);

            return organizationReadDto;
        }



    }
}
