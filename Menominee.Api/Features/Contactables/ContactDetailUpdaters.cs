using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Persons;
using System.Linq;

namespace Menominee.Api.Features.Contactables
{
    public static class ContactDetailUpdaters
    {
        public static void UpdateBusiness(BusinessToWrite businessFromCaller, Business businessFromRepository)
        {
            if (businessFromRepository.Name.ToString() != businessFromCaller.Name.ToString())
                businessFromRepository.SetName(businessFromRepository.Name.NewBusinessName(businessFromCaller.Name.Name).Value);

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

        internal static void UpdatePerson(PersonToWrite personFromCaller, Person personFromRepository)
        {
            UpdateName(personFromCaller, personFromRepository);

            var contactDetails = ContactDetailsFactory
                .Create(personFromCaller.Phones.ToList(), personFromCaller.Emails.ToList(), personFromCaller.Address).Value;

            personFromRepository.UpdateContactDetails(contactDetails);
            personFromRepository.SetNotes(personFromCaller.Notes);
            personFromRepository.SetGender(personFromCaller.Gender);
            personFromRepository.SetBirthday(personFromCaller.Birthday);
            UpdateDriversLicense(personFromCaller, personFromRepository);

        }

        private static void UpdateDriversLicense(PersonToWrite personFromCaller, Person personFromRepository)
        {
            if (personFromCaller?.DriversLicense is not null)
            {
                var result = DateTimeRange.Create(personFromCaller.DriversLicense.Issued, personFromCaller.DriversLicense.Expiry);

                if (result.IsSuccess)
                {
                    var dl = DriversLicense.Create(
                        personFromCaller.DriversLicense.Number,
                        personFromCaller.DriversLicense.State,
                        result.Value).Value;

                    personFromRepository.SetDriversLicense(dl);
                }
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
