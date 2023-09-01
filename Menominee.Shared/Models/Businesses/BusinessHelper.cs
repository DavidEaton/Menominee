using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Businesses
{
    public class BusinessHelper
    {
        public static Business ConvertWriteDtoToEntity(BusinessToWrite business)
        {
            if (business is null)
                return null;

            Address businessAddress = null;
            List<Phone> phones = new();
            List<Email> emails = new();
            BusinessName businessName;

            businessName = BusinessName.Create(business.Name).Value;
            if (business?.Address is not null)
                businessAddress = Address.Create(
                    business.Address.AddressLine1,
                    business.Address.City,
                    business.Address.State,
                    business.Address.AddressLine2,
                    business.Address.PostalCode).Value;

            if (business?.Phones?.Count > 0)
                foreach (var phone in business.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (business?.Emails?.Count > 0)
                foreach (var email in business.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            return Business.Create(businessName,
                                    business.Notes,
                                    PersonHelper.ConvertWriteDtoToEntity(business?.Contact),
                                    businessAddress,
                                    emails,
                                    phones).Value;
        }

        public static BusinessToWrite CovertReadToWriteDto(BusinessToRead business)
        {
            return business is not null
                ? new BusinessToWrite()
                {
                    Name = business.Name,
                    Notes = business?.Notes,
                    Address = AddressHelper.ConvertReadToWriteDto(business.Address),
                    Phones = PhoneHelper.ConvertReadToWriteDtos(business.Phones),
                    Emails = EmailHelper.ConvertReadToWriteDto(business.Emails),
                    Contact = PersonHelper.ConvertReadToWriteDto(business?.Contact)
                }
                : null;
        }

        public static BusinessToRead ConvertToReadDto(Business business)
        {
            return business is not null
                ? new BusinessToRead()
                {
                    Id = business.Id,
                    Name = business.Name.Name,
                    Address = AddressHelper.ConvertToReadDto(business.Address),
                    Notes = business.Notes,
                    Phones = PhoneHelper.ConvertToReadDtos(business.Phones),
                    Emails = EmailHelper.ConvertToReadDtos(business.Emails),
                    Contact = PersonHelper.ConvertToReadDto(business.Contact)
                }
                : null;
        }

        public static BusinessToReadInList ConvertToReadInListDto(Business business)
        {
            return business is not null
                ? new BusinessToReadInList
                {
                    Id = business.Id,
                    Name = business.Name.Name,
                    ContactName = business?.Contact?.Name.LastFirstMiddle,
                    ContactPrimaryPhone = PhoneHelper.GetPrimaryPhone(business?.Contact),

                    AddressLine1 = business?.Address?.AddressLine1,
                    AddressLine2 = business?.Address?.AddressLine2,
                    City = business?.Address?.City,
                    State = business?.Address?.State.ToString(),
                    PostalCode = business?.Address?.PostalCode,

                    Notes = business.Notes,
                    PrimaryPhone = PhoneHelper.GetPrimaryPhone(business),
                    PrimaryPhoneType = PhoneHelper.GetPrimaryPhoneType(business),
                    PrimaryEmail = EmailHelper.GetPrimaryEmail(business)
                }
                : null;
        }
    }
}