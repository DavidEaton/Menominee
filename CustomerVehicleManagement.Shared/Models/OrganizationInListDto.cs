﻿using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Helpers;

namespace CustomerVehicleManagement.Shared.Models
{
    public class OrganizationInListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => $"{AddressLine} {City}, {State}  {PostalCode}"; }
        public string PrimaryPhone { get; set; }
        public string PrimaryPhoneType { get; set; }
        public string PrimaryEmail { get; set; }
        public string Note { get; set; }
        public string ContactName { get; set; }
        public string ContactPrimaryPhone { get; set; }
        public static OrganizationInListDto ConvertToDto(Organization organization)
        {
            if (organization != null)
            {
                return new OrganizationInListDto
                {
                    Id = organization.Id,
                    Name = organization.Name.Name,
                    ContactName = organization?.Contact?.Name.LastFirstMiddle,
                    ContactPrimaryPhone = PhoneHelper.GetPrimaryPhone(organization?.Contact),

                    AddressLine = organization?.Address?.AddressLine,
                    City = organization?.Address?.City,
                    State = organization?.Address?.State,
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
