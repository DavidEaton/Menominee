using Menominee.Domain.Entities;
using Menominee.Domain.ValueObjects;
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
            {
                return null;
            }

            Address businessAddress = null;
            List<Phone> phones = new();
            List<Email> emails = new();
            BusinessName businessName;

            businessName = BusinessName.Create(business.Name.Name).Value;
            if (business?.Address is not null)
            {
                var result = Address.Create(
                    business.Address.AddressLine1,
                    business.Address.City,
                    business.Address.State,
                    business.Address.PostalCode,
                    business.Address.AddressLine2);

                if (result.IsSuccess)
                {
                    businessAddress = result.Value;
                }
            }

            if (business?.Phones?.Count > 0)
            {
                foreach (var phone in business.Phones)
                {
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);
                }
            }

            if (business?.Emails?.Count > 0)
            {
                foreach (var email in business.Emails)
                {
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);
                }
            }

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
                ? new()
                {
                    Name = new BusinessNameRequest() { Name = business.Name },
                    Notes = business?.Notes,
                    Address = AddressHelper.ConvertReadToWriteDto(business?.Address),
                    Phones = PhoneHelper.ConvertReadToWriteDtos(business.Phones),
                    Emails = EmailHelper.ConvertReadToWriteDtos(business.Emails),
                    Contact = PersonHelper.ConvertReadToWriteDto(business?.Contact)
                }
                : new();
        }

        public static BusinessToRead ConvertToReadDto(Business business) =>
            business is null
                ? new()
                : new()
                {
                    Id = business.Id,
                    Name = business.Name?.Name,
                    Notes = business.Notes,
                    Phones = PhoneHelper.ConvertToReadDtos(business.Phones),
                    Emails = EmailHelper.ConvertToReadDtos(business.Emails),
                    Contact = PersonHelper.ConvertToReadDto(business.Contact),
                    Address = MapAddress(business)
                };


        public static BusinessToReadInList ConvertToReadInListDto(Business business) =>
            business is null
                ? new BusinessToReadInList()
                : new BusinessToReadInList
                {
                    Id = business.Id,
                    Name = business.Name?.Name,
                    ContactName = GetContactName(business),
                    Notes = business.Notes,
                    PrimaryPhone = GetPrimaryPhoneNumber(business),
                    PrimaryPhoneType = GetPrimaryPhoneType(business),
                    PrimaryEmail = GetPrimaryEmail(business),
                    AddressLine1 = business.Address?.AddressLine1,
                    AddressLine2 = business.Address?.AddressLine2,
                    City = business.Address?.City,
                    State = business.Address?.State.ToString(),
                };

        private static AddressToRead MapAddress(Business business) =>
            business is null || business.Address is null
                ? new()
                : new()
                {
                    AddressLine1 = business.Address.AddressLine1,
                    AddressLine2 = business.Address.AddressLine2,
                    City = business.Address.City,
                    State = business.Address.State,
                    PostalCode = business.Address.PostalCode
                };

        private static string GetContactName(Business business)
        {
            return business?.Contact?.Name.LastFirstMiddle;
        }

        private static string GetPrimaryPhoneNumber(Business business)
        {
            var phone = PhoneHelper.GetPrimaryPhone(business);

            return phone.IsSuccess
                ? phone.Value.Number.ToString()
                : string.Empty;
        }

        private static string GetPrimaryPhoneType(Business business)
        {
            var phone = PhoneHelper.GetPrimaryPhone(business);

            return phone.IsSuccess
                ? phone.Value.PhoneType.ToString()
                : string.Empty;
        }

        private static string GetPrimaryEmail(Business business)
        {
            return EmailHelper.GetPrimaryEmail(business);
        }


        internal static BusinessToWrite ConvertToWriteDto(Business business)
        {
            return business is not null
                ? new()
                {
                    Id = business.Id,
                    Name = new BusinessNameRequest() { Name = business.Name.Name },
                    Notes = business.Notes,
                    Contact = PersonHelper.ConvertToWriteDto(business?.Contact),
                    Address = AddressHelper.ConvertToWriteDto(business?.Address),
                    Phones = PhoneHelper.ConvertToWriteDtos(business.Phones),
                    Emails = EmailHelper.ConvertToWriteDtos(business.Emails)
                }
                : new();
        }

        internal static BusinessToWrite ConvertReadToWriteDto(BusinessToRead business)
        {
            return business is not null
                ? new()
                {
                    Id = business.Id,
                    Name = new BusinessNameRequest() { Name = business.Name },
                    Notes = business.Notes,
                    Contact = PersonHelper.ConvertReadToWriteDto(business?.Contact),
                    Address = AddressHelper.ConvertReadToWriteDto(business?.Address),
                    Phones = PhoneHelper.ConvertReadToWriteDtos(business.Phones),
                    Emails = EmailHelper.ConvertReadToWriteDtos(business.Emails)
                }
                : new();
        }

        internal static BusinessToRead CovertWriteToReadDto(BusinessToWrite business)
        {
            return business is not null
                ? new()
                {
                    Id = business.Id,
                    Name = business.Name.Name,
                    Notes = business.Notes,
                    Contact = PersonHelper.ConvertWriteToReadDto(business?.Contact),
                    Address = AddressHelper.ConvertWriteToReadDto(business?.Address),
                    Phones = PhoneHelper.ConvertWriteToReadDtos(business.Phones),
                    Emails = EmailHelper.ConvertWriteToReadDtos(business.Emails)
                }
                : new();
        }
    }
}