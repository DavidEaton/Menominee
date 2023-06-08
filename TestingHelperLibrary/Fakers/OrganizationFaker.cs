using Bogus;
using Menominee.Common.ValueObjects;
using Organization = CustomerVehicleManagement.Domain.Entities.Organization;
using Person = CustomerVehicleManagement.Domain.Entities.Person;

namespace TestingHelperLibrary.Fakers
{
    public class OrganizationFaker : Faker<Organization>
    {
        public OrganizationFaker(bool generateId, bool includeAddress = false, bool includeContact = false, int emailsCount = 0, int phonesCount = 0)
        {
            RuleFor(entity => entity.Id, faker => generateId ? faker.Random.Long(1, 10000) : 0);

            CustomInstantiator(faker =>
            {
                var name = OrganizationName.Create(faker.Company.CompanyName()).Value;
                var notes = faker.Lorem.Sentence(20);
                var birthday = faker.Person.DateOfBirth;

                Address? address = null;
                Person? contact = null;

                if (includeAddress)
                    address = new AddressFaker().Generate();

                if (includeContact)
                    contact = new PersonFaker(true, includeAddress, false, emailsCount, phonesCount).Generate();

                var emails = new EmailFaker(generateId).Generate(emailsCount);
                var phones = new PhoneFaker(generateId).Generate(phonesCount);

                var result = Organization.Create(name, notes, contact, address, emails, phones);

                return result.IsSuccess ? result.Value : throw new InvalidOperationException(result.Error);
            });
        }
    }
}
