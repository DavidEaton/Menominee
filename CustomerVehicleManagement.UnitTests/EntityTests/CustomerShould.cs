using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using Xunit;
using FluentAssertions;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class CustomerShould
    {
        [Fact]
        public void CreateCustomerWithPersonEntity()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);

            // Act
            var customer = new Customer(person);

            // Assert
            customer.Entity.Should().BeOfType<Person>();
        }

        [Fact]
        public void HaveCreatedDateOnCreated()
        {
            var organization = CreateValidOrganization();

            var customer = new Customer(organization);

            customer.Created.Should().BeCloseTo(DateTime.UtcNow);
        }

        [Fact]
        public void CreateCustomerWithOrganizationEntity()
        {
            var organization = CreateValidOrganization();

            var customer = new Customer(organization);

            customer.Entity.Should().BeOfType<Organization>();
        }


        [Fact]
        public void CreatePersonCustomerWithAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var address = new Address(addressLine, city, state, postalCode);
            var person = new Person(name, Gender.Female, null, address);

            var customer = new Customer(person);
            var jane = (Person)customer.Entity;

            jane.Name.LastFirstMiddle.Should().Be($"{lastName}, {firstName}");
            jane.Address.AddressLine.Should().Be(addressLine);
            jane.Address.City.Should().Be(city);
            jane.Address.State.Should().Be(state);
            jane.Address.PostalCode.Should().Be(postalCode);
        }

        [Fact]
        public void CreateOrganizationCustomerWithAddress()
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = "MI";
            var postalCode = "49735";
            var address = new Address(addressLine, city, state, postalCode);
            var name = "Jane's";
            var organization = new Organization(name, address);

            var customer = new Customer(organization);
            var janes = (Organization)customer.Entity;

            customer.Entity.Should().BeOfType<Organization>();
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
            var name = new PersonName(lastName, firstName);
            var person = new Person(name, Gender.Female);
            var organization = CreateValidOrganization();
            organization.SetContact(person);

            var customer = new Customer(organization);
            var janes = (Organization)customer.Entity;

            janes.Contact.Should().BeOfType<Person>();
            janes.Contact.Name.LastFirstMiddle.Should().Be($"{lastName}, {firstName}");
        }

        [Fact]
        public void AddPhones()
        {
            var organization = CreateValidOrganization();
            var customer = new Customer(organization);
            var number0 = "(989) 627-9206";
            var phone0 = new Phone(number0, PhoneType.Mobile, true);
            var number1 = "(231) 675-1922";
            var phone1 = new Phone(number1, PhoneType.Mobile, false);

            customer.AddPhone(phone0);
            customer.AddPhone(phone1);

            customer.Phones.Should().Contain(phone0);
            customer.Phones.Should().Contain(phone1);
        }

        [Fact]
        public void RemovePhones()
        {
            var organization = CreateValidOrganization();
            var customer = new Customer(organization);
            var number0 = "(989) 627-9206";
            var phone0 = new Phone(number0, PhoneType.Mobile, true);
            var number1 = "(231) 675-1922";
            var phone1 = new Phone(number1, PhoneType.Mobile, false);

            customer.AddPhone(phone0);
            customer.AddPhone(phone1);
            customer.Phones.Should().Contain(phone0);
            customer.Phones.Should().Contain(phone1);
            customer.RemovePhone(phone0);
            customer.RemovePhone(phone1);

            customer.Phones.Count.Should().Be(0);
        }

        [Fact]
        public void AddVehicles()
        {
            var organization = CreateValidOrganization();
            var customer = new Customer(organization);
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
            var organization = CreateValidOrganization();
            var customer = new Customer(organization);
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

        private static Organization CreateValidOrganization()
        {
            var name = "Jane's";
            var organization = new Organization(name);

            return organization;
        }
    }
}

