using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Enums;
using CustomerVehicleManagement.Domain.ValueObjects;
using NUnit.Framework;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Tests.EntityTests
{
    public class CustomerShould
    {
        [Test]
        public void CreateNewCustomerWithPersonEntity()
        {
            // Arrange
            string firstName = "Jane";
            string lastName = "Doe";

            // Act
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            // Assert
            var customer = new Customer(person);
            Assert.That(customer.Entity is Person);
        }

        [Test]
        public void CreateNewCustomerWithOrganizationEntity()
        {
            string name = "Jane's";
            var organization = new Organization(name);

            var customer = new Customer(organization);
            Assert.That(customer.Entity is Organization);
        }

        [Test]
        public void CreateNewPersonCustomerWithAddress()
        {
            string addressLine = "1234 Five Street";
            string city = "Gaylord";
            string state = "MI";
            string postalCode = "49735";
            string firstName = "Jane";
            string lastName = "Doe";

            var name = new PersonName(lastName, firstName);
            var address = new Address(addressLine, city, state, postalCode);

            var person = new Person(name, Gender.Female, null, address);

            var customer = new Customer(person);
            Person jane = (Person)customer.Entity;

            Assert.That($"{lastName}, {firstName}", Is.EqualTo(jane.Name.LastFirstMiddle));
            Assert.That(addressLine, Is.EqualTo(jane.Address.AddressLine));
            Assert.That(city, Is.EqualTo(jane.Address.City));
            Assert.That(state, Is.EqualTo(jane.Address.State));
            Assert.That(postalCode, Is.EqualTo(jane.Address.PostalCode));
        }

        [Test]
        public void CreateNewOrganizationCustomerWithAddress()
        {
            string name = "Jane's";
            var organization = new Organization(name);

            var customer = new Customer(organization);
            Assert.That(customer.Entity is Organization);

            Organization janes = (Organization)customer.Entity;

            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";

            janes.Address = new Address(addressLine, city, state, postalCode);

            Assert.That(addressLine, Is.EqualTo(janes.Address.AddressLine));
            Assert.That(city, Is.EqualTo(janes.Address.City));
            Assert.That(state, Is.EqualTo(janes.Address.State));
            Assert.That(postalCode, Is.EqualTo(janes.Address.PostalCode));
        }

        [Test]
        public void CreateNewOrganizationCustomerWithPersonContact()
        {
            string janesAuto = "Jane's Auto";
            string firstName = "Jane";
            string lastName = "Doe";

            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            var organization = new Organization(janesAuto)
            {
                Contact = person
            };

            var customer = new Customer(organization);
            Organization janes = (Organization)customer.Entity;

            Assert.That(janes.Contact is Person);
            Assert.That(janes.Contact.Name.LastFirstMiddle == $"{lastName}, {firstName}");
        }

        [Test]
        public void AddPhones()
        {
            string name = "Jane's";
            var organization = new Organization(name);

            var customer = new Customer(organization);

            string number0 = "(989) 627-9206";
            Phone phone0 = new Phone(number0, PhoneType.Mobile);

            string number1 = "(231) 675-1922";
            Phone phone1 = new Phone(number1, PhoneType.Mobile);

            customer.AddPhone(phone0);
            customer.AddPhone(phone1);

            Assert.That(customer, Is.Not.Null);
            Assert.That(customer.Phones, Is.Not.Null);
            Assert.That(customer.Phones[0].Number, Is.EqualTo(number0));
            Assert.That(customer.Phones[1].Number, Is.EqualTo(number1));
        }

        [Test]
        public void RemovePhones()
        {
            string name = "Jane's";
            var organization = new Organization(name);

            var customer = new Customer(organization);

            string number0 = "(989) 627-9206";
            Phone phone0 = new Phone(number0, PhoneType.Mobile);

            string number1 = "(231) 675-1922";
            Phone phone1 = new Phone(number1, PhoneType.Mobile);

            customer.AddPhone(phone0);
            customer.AddPhone(phone1);

            Assert.That(customer.Phones[0].Number, Is.EqualTo(number0));
            Assert.That(customer.Phones[1].Number, Is.EqualTo(number1));

            customer.RemovePhone(phone0);
            customer.RemovePhone(phone1);
            Assert.That(customer.Phones.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddVehicles()
        {
            string name = "Jane's";
            var organization = new Organization(name);
            var customer = new Customer(organization);

            string vin0 = "45kj64k64kjyvrv";
            int year0 = 2020;
            string make0 = "Honda";
            string model0 = "Pilot";
            Vehicle vehicle0 = new Vehicle(vin0, year0, make0, model0, customer);

            string vin1 = "547hjg54lgg274bg";
            int year1 = 2010;
            string make1 = "Jeep";
            string model1 = "Jeepers";
            Vehicle vehicle1 = new Vehicle(vin1, year1, make1, model1, customer);

            customer.AddVehicle(vehicle0);
            customer.AddVehicle(vehicle1);

            Assert.That(customer.Vehicles, Is.Not.Null);
            Assert.That(customer.Vehicles[0], Is.EqualTo(vehicle0));
            Assert.That(customer.Vehicles[1], Is.EqualTo(vehicle1));
        }


    }
}