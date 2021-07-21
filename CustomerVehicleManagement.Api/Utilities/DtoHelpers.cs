using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using SharedKernel.Enums;
using System.Collections.Generic;
using Helper = CustomerVehicleManagement.Api.Utilities.ContactableHelpers;


namespace CustomerVehicleManagement.Api.Utilities
{
    public static class DtoHelpers
    {
        public static PersonInListDto PersonToPersonInListDto(Person person)
        {
            return new PersonInListDto()
            {
                AddressLine = person?.Address?.AddressLine,
                Birthday = person?.Birthday,
                City = person?.Address?.City,
                Id = person.Id,
                Name = person.Name.LastFirstMiddle,
                PostalCode = person?.Address?.PostalCode,
                State = person?.Address?.State,
                PrimaryPhone = Helper.GetPrimaryPhone(person) ?? Helper.GetOrdinalPhone(person, 0),
                PrimaryPhoneType = Helper.GetPrimaryPhoneType(person) ?? Helper.GetOrdinalPhoneType(person, 0),
                PrimaryEmail = Helper.GetPrimaryEmail(person) ?? Helper.GetOrdinalEmail(person, 0)
            };
        }

        internal static CustomerInListDto CustomerToCustomerInListDto(Customer customer)
        {
            CustomerInListDto customerInListDto = new();
            customerInListDto.Id = customer.Id;
            customerInListDto.EntityType = customer.EntityType;
            customerInListDto.EntityId = customer.EntityType == EntityType.Organization
                                                                                        ? customer.Organization.Id
                                                                                        : customer.Person.Id;
            customerInListDto.CustomerType = customer.CustomerType.ToString();

            if (customer.EntityType == EntityType.Organization)
            {
                customerInListDto.AddressFull = customer.Organization?.Address?.AddressLine;
                customerInListDto.Name = customer.Organization?.Name?.Name;
                //customerInListDto.PrimaryPhone = Helper.GetPrimaryPhone(organization) ?? Helper.GetOrdinalPhone(organization, 0);
                //customerInListDto.PrimaryPhoneType = Helper.GetPrimaryPhoneType(organization) ?? Helper.GetOrdinalPhoneType(organization, 0);
                //customerInListDto.PrimaryEmail = Helper.GetPrimaryEmail(organization) ?? Helper.GetOrdinalEmail(organization, 0);

                //customerInListDto.ContactName = organization?.Contact?.Name.LastFirstMiddle;
                //customerInListDto.ContactPrimaryPhone = Helper.GetPrimaryPhone(organization?.Contact)
                //                                     ?? Helper.GetOrdinalPhone(organization?.Contact, 0);
                //customerInListDto.ContactPrimaryPhoneType = Helper.GetPrimaryPhoneType(organization?.Contact)
                //                                     ?? Helper.GetOrdinalPhoneType(organization?.Contact, 0);
            }

            if (customer.EntityType == EntityType.Person)
            {
                customerInListDto.AddressFull = customer.Person?.Address?.AddressLine;
                customerInListDto.Name = customer.Person?.Name.LastFirstMiddle;
                customerInListDto.PrimaryPhone = Helper.GetPrimaryPhone(customer.Person) ?? Helper.GetOrdinalPhone(customer.Person, 0);
                customerInListDto.PrimaryPhoneType = Helper.GetPrimaryPhoneType(customer.Person) ?? Helper.GetOrdinalPhoneType(customer.Person, 0);
                customerInListDto.PrimaryEmail = Helper.GetPrimaryEmail(customer.Person) ?? Helper.GetOrdinalEmail(customer.Person, 0);
            }

            return customerInListDto;
        }

        public static IList<Phone> PhonesUpdateDtoToPhones(IList<PhoneUpdateDto> phoneUpdateDtos)
        {
            var phones = new List<Phone>();

            if (phoneUpdateDtos != null)
            {
                foreach (var phone in phoneUpdateDtos)
                    phones.Add(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));
            }

            return phones;
        }

        public static IList<Email> EmailsUpdateDtoToEmails(IList<EmailUpdateDto> emailUpdateDtos)
        {
            var emails = new List<Email>();

            if (emailUpdateDtos != null)
            {
                foreach (var email in emailUpdateDtos)
                    emails.Add(new Email(email.Address, email.IsPrimary));
            }

            return emails;
        }


        /// <summary>
        /// Map the PersonUpdateDto back to the domain entity
        /// </summary>
        /// <param name="personUpdateDto"></param>
        /// <param name="person"></param>
        public static void PersonUpdateDtoToPerson(PersonUpdateDto personUpdateDto,
                                                               Person person)
        {
            person.SetName(personUpdateDto.Name);
            person.SetGender(personUpdateDto.Gender);

            if (personUpdateDto.Address != null)
                person.SetAddress(personUpdateDto.Address);

            person.SetBirthday(personUpdateDto.Birthday);
            person.SetDriversLicense(personUpdateDto.DriversLicense);
            person.SetPhones(PhonesUpdateDtoToPhones(personUpdateDto.Phones));
            person.SetEmails(EmailsUpdateDtoToEmails(personUpdateDto.Emails));
        }
    }
}
