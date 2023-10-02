using Menominee.Api.Persons;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Persons;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Api.Common
{
    public static class Updaters
    {
        public static void UpdateBusiness(BusinessToWrite businessFromCaller, Business businessFromRepository)
        {
            if (businessFromRepository.Name.Name != businessFromCaller.Name)
                businessFromRepository.SetName(businessFromRepository.Name.NewBusinessName(businessFromCaller.Name).Value);

            if (businessFromCaller?.Address is not null)
            {
                businessFromRepository.SetAddress(Address.Create(
                        businessFromCaller.Address.AddressLine1,
                        businessFromCaller.Address.City,
                        businessFromCaller.Address.State,
                        businessFromCaller.Address.PostalCode,
                        businessFromCaller.Address.AddressLine2)
                    .Value);
            }

            // Client may send an empty or null Address VALUE OBJECT, signifying REMOVAL
            // TODO: Get Al's input on this. I think it's fine to remove the address if
            // the client sends an empty or null Address VALUE OBJECT -DE
            if (businessFromCaller?.Address is null)
                businessFromRepository.ClearAddress();

            businessFromRepository.SetNotes(businessFromCaller.Notes);

            // Client may send an empty or null phones collection of ENTITIES, signifying REMOVAL
            // TODO: Get Al's input on this. I think it's fine... -DE
            foreach (var phone in businessFromCaller?.Phones)
            {
                if (phone.Id == 0)
                    businessFromRepository.AddPhone(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

                if (phone.Id != 0)
                {
                    var contextPhone = businessFromRepository.Phones.FirstOrDefault(contextPhone => contextPhone.Id == phone.Id);
                    contextPhone.SetNumber(phone.Number);
                    contextPhone.SetIsPrimary(phone.IsPrimary);
                    contextPhone.SetPhoneType(phone.PhoneType);
                }

                if (phone.Id != 0)
                    businessFromRepository.RemovePhone(
                        businessFromRepository.Phones.FirstOrDefault(
                            contextPhone =>
                            contextPhone.Id == phone.Id));
            }

            // Client may send an empty or null emails collection, signifying REMOVAL
            // TODO: Get Al's input on this. I think it's fine... -DE
            List<Email> emails = new();
            if (businessFromCaller?.Emails.Count > 0)
            {
                emails.AddRange(businessFromCaller.Emails
                    .Select(email =>
                            Email.Create(email.Address,
                                         email.IsPrimary).Value));
            }

            var contactDetails = ContactDetailsFactory.Create(
                businessFromCaller.Phones, businessFromCaller.Emails, businessFromCaller.Address).Value;

            businessFromRepository.UpdateContactDetails(contactDetails);

            businessFromRepository.SetNotes(businessFromCaller.Notes);

            // Contact
            if (businessFromCaller?.Contact is not null && businessFromCaller.Contact.IsNotEmpty)
            {
                //var result = await personsController.UpdateAsync(businessFromCaller.Contact);

                //var person = await personsRepository.GetEntityAsync(businessFromRepository.Contact.Id);

                //if (person != null)
                //    businessFromRepository.SetContact(person);
            }
        }

        internal static void UpdatePerson(PersonToWrite personFromCaller, Person personFromRepository, IPersonRepository repository)
        {
            UpdateName(personFromCaller, personFromRepository);
            var contactDetails = ContactDetailsFactory
                .Create(personFromCaller.Phones.ToList(), personFromCaller.Emails.ToList(), personFromCaller.Address).Value;

            var phonesToDelete = personFromRepository.Phones
                .Where(phone => !contactDetails.Phones.Any(phoneToKeep => phoneToKeep.Id == phone.Id))
                .ToList();

            if (phonesToDelete.Any())
            {
                phonesToDelete.ForEach(phone => repository.DeletePhone(phone));
            }

            var emailsToDelete = personFromRepository.Emails
                .Where(email => !contactDetails.Emails.Any(emailToKeep => emailToKeep.Id == email.Id))
                .ToList();

            if (emailsToDelete.Any())
            {
                emailsToDelete.ForEach(email => repository.DeleteEmail(email));
            }

            personFromRepository.UpdateContactDetails(contactDetails);

            personFromRepository.SetGender(personFromCaller.Gender);
            personFromRepository.SetBirthday(personFromCaller.Birthday);

            if (personFromCaller?.DriversLicense is not null)
            {
                personFromRepository.SetDriversLicense(DriversLicense.Create(personFromCaller.DriversLicense.Number,
                    personFromCaller.DriversLicense.State,
                    DateTimeRange.Create(
                    personFromCaller.DriversLicense.Issued,
                    personFromCaller.DriversLicense.Expiry).Value).Value);
            }

        }


        internal static void UpdateName(PersonToWrite personFromCaller, Person personFromRepository)
        {
            // Data Transfer Objects have been validated in ASP.NET request pipeline
            if (NamesAreNotEqual(personFromRepository.Name, personFromCaller.Name))
            {
                personFromRepository.SetName(PersonName.Create(
                    personFromCaller.Name.LastName,
                    personFromCaller.Name.FirstName,
                    personFromCaller.Name.MiddleName)
                    .Value);
            }
        }

        internal static bool NamesAreNotEqual(PersonName name, PersonNameToWrite nameDto) =>
            !string.Equals(name.FirstName, nameDto?.FirstName) ||
            !string.Equals(name.MiddleName, nameDto?.MiddleName) ||
            !string.Equals(name.LastName, nameDto?.LastName);
    }
}
