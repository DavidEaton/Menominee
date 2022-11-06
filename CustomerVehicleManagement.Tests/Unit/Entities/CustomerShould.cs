using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Organizations;
using CustomerVehicleManagement.Shared;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class CustomerShould
    {
        [Fact]
        public void CreateCustomerWithPersonEntity()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var person = Person.Create(name, Gender.Female).Value;

            // Act
            var customer = new Customer(person, CustomerType.Retail);

            // Assert
            customer.Should().BeOfType<Customer>();
            customer.EntityType.Should().Be(EntityType.Person);
        }

        [Fact]
        public void CreateCustomerWithOrganizationEntity()
        {
            var organization = OrganizationHelper.CreateTestOrganization();

            var customer = new Customer(organization, CustomerType.Retail);
            customer.EntityType.Should().Be(EntityType.Organization);
        }

        [Fact]
        public void SetAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var addressOrError = Address.Create(addressLine, city, state, postalCode);
            var organization = OrganizationHelper.CreateTestOrganization();

            organization.SetAddress(addressOrError.Value);
            var customer = new Customer(organization, CustomerType.Retail);
            var janes = customer.Organization;

            customer.EntityType.Should().Be(EntityType.Organization);
            janes.Address.AddressLine.Should().Be(addressLine);
            janes.Address.City.Should().Be(city);
            janes.Address.State.Should().Be(state);
            janes.Address.PostalCode.Should().Be(postalCode);
        }

        [Fact]
        public void CreateOrganizationCustomerWithPersonContact()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var person = Person.Create(name, Gender.Female).Value;
            var organization = OrganizationHelper.CreateTestOrganization();
            organization.SetContact(person);

            var customer = new Customer(organization, CustomerType.Retail);
            var janes = customer.Organization;

            janes.Contact.Should().BeOfType<Person>();
            janes.Contact.Name.LastFirstMiddle.Should().Be($"{lastName}, {firstName}");
        }

        [Fact]
        public void AddOrganizationPhones()
        {
            var organization = OrganizationHelper.CreateTestOrganization();
            var customer = new Customer(organization, CustomerType.Retail);
            var number0 = "(989) 627-9206";
            var phone0 = Phone.Create(number0, PhoneType.Mobile, true).Value;
            var number1 = "(231) 675-1922";
            var phone1 = Phone.Create(number1, PhoneType.Mobile, false).Value;

            customer.Organization.AddPhone(phone0);
            customer.Organization.AddPhone(phone1);

            customer.Organization.Phones.Should().Contain(phone0);
            customer.Organization.Phones.Should().Contain(phone1);
        }

        [Fact]
        public void RemoveOrganizationPhones()
        {
            var organization = OrganizationHelper.CreateTestOrganization();
            var customer = new Customer(organization, CustomerType.Retail);
            var number0 = "(989) 627-9206";
            var phone0 = Phone.Create(number0, PhoneType.Mobile, true).Value;
            var number1 = "(231) 675-1922";
            var phone1 = Phone.Create(number1, PhoneType.Mobile, false).Value;

            customer.Organization.AddPhone(phone0);
            customer.Organization.AddPhone(phone1);
            customer.Organization.Phones.Should().Contain(phone0);
            customer.Organization.Phones.Should().Contain(phone1);
            customer.Organization.RemovePhone(phone0);
            customer.Organization.RemovePhone(phone1);

            customer.Organization.Phones.Count.Should().Be(0);
        }

        [Fact]
        public void AddOrganizationEmails()
        {
            var organization = OrganizationHelper.CreateTestOrganization();
            var customer = new Customer(organization, CustomerType.Retail);
            var email0 = Email.Create("mary@moops.com", true).Value;
            var email1 = Email.Create("mikey@yikes.com", false).Value;

            customer.Organization.AddEmail(email0);
            customer.Organization.AddEmail(email1);

            customer.Organization.Emails.Should().Contain(email0);
            customer.Organization.Emails.Should().Contain(email1);
        }

        [Fact]
        public void RemoveOrganizationEmails()
        {
            var organization = OrganizationHelper.CreateTestOrganization();
            var customer = new Customer(organization, CustomerType.Retail);
            var email0 = Email.Create("mary@moops.com", true).Value;
            var email1 = Email.Create("mikey@yikes.com", false).Value;

            customer.Organization.AddEmail(email0);
            customer.Organization.AddEmail(email1);
            customer.Organization.Emails.Should().Contain(email0);
            customer.Organization.Emails.Should().Contain(email1);
            customer.Organization.RemoveEmail(email0);
            customer.Organization.RemoveEmail(email1);

            customer.Organization.Emails.Count.Should().Be(0);
        }

        [Fact]
        public void AddPersonPhones()
        {
            var organization = Utilities.CreatePerson();
            var customer = new Customer(organization, CustomerType.Retail);
            var number0 = "(989) 627-9206";
            var phone0 = Phone.Create(number0, PhoneType.Mobile, true).Value;
            var number1 = "(231) 675-1922";
            var phone1 = Phone.Create(number1, PhoneType.Mobile, false).Value;

            customer.Person.AddPhone(phone0);
            customer.Person.AddPhone(phone1);

            customer.Person.Phones.Should().Contain(phone0);
            customer.Person.Phones.Should().Contain(phone1);
        }

        [Fact]
        public void RemovePersonPhones()
        {
            var organization = Utilities.CreatePerson();
            var customer = new Customer(organization, CustomerType.Retail);
            var number0 = "(989) 627-9206";
            var phone0 = Phone.Create(number0, PhoneType.Mobile, true).Value;
            var number1 = "(231) 675-1922";
            var phone1 = Phone.Create(number1, PhoneType.Mobile, false).Value;

            customer.Person.AddPhone(phone0);
            customer.Person.AddPhone(phone1);
            customer.Person.Phones.Should().Contain(phone0);
            customer.Person.Phones.Should().Contain(phone1);
            customer.Person.RemovePhone(phone0);
            customer.Person.RemovePhone(phone1);

            customer.Person.Phones.Count.Should().Be(0);
        }

        [Fact]
        public void DisallowAddDuplicatePhone()
        {

        }

        [Fact]
        public void AddPersonEmails()
        {
            var organization = Utilities.CreatePerson();
            var customer = new Customer(organization, CustomerType.Retail);
            var email0 = Email.Create("mary@moops.com", true).Value;
            var email1 = Email.Create("mikey@yikes.com", false).Value;

            customer.Person.AddEmail(email0);
            customer.Person.AddEmail(email1);

            customer.Person.Emails.Should().Contain(email0);
            customer.Person.Emails.Should().Contain(email1);
        }

        [Fact]
        public void RemovePersonEmails()
        {
            var organization = Utilities.CreatePerson();
            var customer = new Customer(organization, CustomerType.Retail);
            var email0 = Email.Create("mary@moops.com", true).Value;
            var email1 = Email.Create("mikey@yikes.com", false).Value;

            customer.Person.AddEmail(email0);
            customer.Person.AddEmail(email1);
            customer.Person.Emails.Should().Contain(email0);
            customer.Person.Emails.Should().Contain(email1);
            customer.Person.RemoveEmail(email0);
            customer.Person.RemoveEmail(email1);

            customer.Person.Emails.Count.Should().Be(0);
        }

        [Fact]
        public void AddVehicles()
        {
            var organization = OrganizationHelper.CreateTestOrganization();
            var customer = new Customer(organization, CustomerType.Retail);
            var vin0 = "45kj64k64kjyvrv";
            var year0 = 2020;
            var make0 = "Honda";
            var model0 = "Pilot";
            var vehicle0 = new Vehicle(vin0, year0, make0, model0, customer);
            var vin1 = "547hjg54lgg274bg";
            var year1 = 2010;
            var make1 = "Jeep";
            var model1 = "Jeepers";
            var vehicle1 = new Vehicle(vin1, year1, make1, model1, customer);

            customer.AddVehicle(vehicle0);
            customer.AddVehicle(vehicle1);

            customer.Vehicles.Should().Contain(vehicle0);
            customer.Vehicles.Should().Contain(vehicle1);

        }

        [Fact]
        public void RemoveVehicle()
        {
            var organization = OrganizationHelper.CreateTestOrganization();
            var customer = new Customer(organization, CustomerType.Retail);
            var vin0 = "45kj64k64kjyvrv";
            var year0 = 2020;
            var make0 = "Honda";
            var model0 = "Pilot";
            var vehicle0 = new Vehicle(vin0, year0, make0, model0, customer);
            var vin1 = "547hjg54lgg274bg";
            var year1 = 2010;
            var make1 = "Jeep";
            var model1 = "Jeepers";
            var vehicle1 = new Vehicle(vin1, year1, make1, model1, customer);
            customer.AddVehicle(vehicle0);
            customer.AddVehicle(vehicle1);

            customer.Vehicles.Count.Should().Be(2);
            customer.RemoveVehicle(vehicle0);

            customer.Vehicles.Count.Should().Be(1);
        }
    }
}

