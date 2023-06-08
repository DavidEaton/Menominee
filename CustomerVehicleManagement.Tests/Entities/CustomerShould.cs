using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;
using TestingHelperLibrary;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class CustomerShould
    {
        [Fact]
        public void Create_Customer_With_Person_Entity()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var notes = Utilities.LoremIpsum(100);
            var person = Person.Create(name, Gender.Female, notes).Value;

            // Act
            var customer = Customer.Create(person, CustomerType.Retail).Value;

            // Assert
            customer.Should().BeOfType<Customer>();
            customer.EntityType.Should().Be(EntityType.Person);
        }

        [Fact]
        public void CreateCustomerWithOrganizationEntity()
        {
            var organization = ContactableTestHelper.CreateOrganization();

            var customer = Customer.Create(organization, CustomerType.Retail).Value;
            customer.EntityType.Should().Be(EntityType.Organization);
        }

        [Fact]
        public void Not_Create_Customer_With_Null_Person_Entity()
        {
            Person person = null;

            var result = Customer.Create(person, CustomerType.Retail);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Fact]
        public void Not_Create_Person_Customer_With_Invalid_CustomerType()
        {
            var person = ContactableTestHelper.CreatePerson();
            var invalidCustomerType = (CustomerType)(-1);

            var result = Customer.Create(person, invalidCustomerType);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Fact]
        public void Not_Create_Organization_Customer_With_Null_Organization_Entity()
        {
            Organization organization = null;

            var result = Customer.Create(organization, CustomerType.Retail);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Fact]
        public void Not_Create_Organization_Customer_With_Invalid_CustomerType()
        {
            var organization = ContactableTestHelper.CreateOrganization();
            var invalidCustomerType = (CustomerType)(-1);

            var result = Customer.Create(organization, invalidCustomerType);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void SetAddress(Customer customer)
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var address = Address.Create(addressLine, city, state, postalCode).Value;

            var result = customer.SetAddress(address);
            result.IsSuccess.Should().BeTrue();
            customer.Address.Should().Be(address);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Set_Null_Address(Customer customer)
        {
            Address address = null;

            var result = customer.SetAddress(address);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void ClearAddress(Customer customer)
        {
            var addressLine = "1234 Five Street";
            var city = "Gaylord";
            var state = State.MI;
            var postalCode = "49735";
            var address = Address.Create(addressLine, city, state, postalCode).Value;
            customer.SetAddress(address);
            customer.Address.Should().NotBeNull();
            customer.Address.Should().Be(address);

            var result = customer.ClearAddress();

            result.IsSuccess.Should().BeTrue();
            customer.Address.Should().BeNull();
        }

        [Fact]
        public void CreateOrganizationCustomerWithPersonContact()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = PersonName.Create(lastName, firstName).Value;
            var notes = Utilities.LoremIpsum(100);
            var person = Person.Create(name, Gender.Female, notes).Value;
            var organization = ContactableTestHelper.CreateOrganization();
            organization.SetContact(person);

            var customer = Customer.Create(organization, CustomerType.Retail).Value;
            var contact = customer.Contact;

            contact.Should().BeOfType<Person>();
            contact.Should().Be(customer.Contact);
            contact.Name.LastFirstMiddle.Should().Be($"{lastName}, {firstName}");
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void AddPhone(Customer customer)
        {
            var phone0 = Phone.Create("(555) 123-4567", PhoneType.Mobile, true).Value;
            var phone1 = Phone.Create("(555) 987-6543", PhoneType.Mobile, false).Value;

            customer.AddPhone(phone0);
            customer.AddPhone(phone1);

            customer.Phones.Should().Contain(phone0);
            customer.Phones.Should().Contain(phone1);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Add_Null_Phone(Customer customer)
        {
            var phone0 = Phone.Create("(555) 123-4567", PhoneType.Mobile, true).Value;
            var phone1 = Phone.Create("(555) 987-6543", PhoneType.Mobile, false).Value;
            customer.AddPhone(phone0);
            customer.AddPhone(phone1);
            customer.Phones.Should().Contain(phone0);
            customer.Phones.Should().Contain(phone1);
            Phone nullPhone = null;

            var result = customer.AddPhone(nullPhone);
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void RemovePhone(Customer customer)
        {
            var phone0 = Phone.Create("(555) 123-4567", PhoneType.Mobile, true).Value;
            var phone1 = Phone.Create("(555) 987-6543", PhoneType.Mobile, false).Value;

            customer.AddPhone(phone0);
            customer.AddPhone(phone1);
            customer.Phones.Should().Contain(phone0);
            customer.Phones.Should().Contain(phone1);
            customer.Phones.Count.Should().Be(2);
            customer.RemovePhone(phone0);
            customer.RemovePhone(phone1);
            customer.Phones.Count.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Remove_Null_Phone(Customer customer)
        {
            var phone0 = Phone.Create("(555) 123-4567", PhoneType.Mobile, true).Value;
            var phone1 = Phone.Create("(555) 987-6543", PhoneType.Mobile, false).Value;
            customer.AddPhone(phone0);
            customer.AddPhone(phone1);
            customer.Phones.Should().Contain(phone0);
            customer.Phones.Should().Contain(phone1);
            Phone nullPhone = null;

            var result = customer.RemovePhone(nullPhone);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Allow_Add_Duplicate_Phone(Customer customer)
        {
            var phone0 = Phone.Create("(555) 123-4567", PhoneType.Mobile, true).Value;
            customer.AddPhone(phone0);

            var result = customer.AddPhone(phone0);
            result.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void AddEmail(Customer customer)
        {
            var email = Email.Create("mary@moops.com", true).Value;

            customer.AddEmail(email);

            customer.Emails.Should().Contain(email);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Add_Null_Email(Customer customer)
        {
            var email = Email.Create("mikey@yikes.com", false).Value;
            customer.AddEmail(email);
            customer.Emails.Should().Contain(email);
            Email nullEmail = null;

            var result = customer.AddEmail(nullEmail);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);

        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Add_Duplicate_Email(Customer customer)
        {
            var email = Email.Create("mary@moops.com", true).Value;

            customer.AddEmail(email);
            var resut = customer.AddEmail(email);

            resut.IsFailure.Should().BeTrue();
            resut.Error.Should().Contain(Customer.DuplicateItemMessagePrefix);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void RemoveEmail(Customer customer)
        {
            var email0 = Email.Create("mary@moops.com", true).Value;
            var email1 = Email.Create("mikey@yikes.com", false).Value;

            customer.AddEmail(email0);
            customer.AddEmail(email1);
            customer.Emails.Should().Contain(email0);
            customer.Emails.Should().Contain(email1);
            customer.Emails.Count.Should().Be(2);
            customer.RemoveEmail(email0);
            customer.RemoveEmail(email1);

            customer.Emails.Count.Should().Be(0);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Remove_Null_Email(Customer customer)
        {
            var email0 = Email.Create("mary@moops.com", true).Value;
            var email1 = Email.Create("mikey@yikes.com", false).Value;
            customer.AddEmail(email0);
            customer.AddEmail(email1);
            customer.Emails.Should().Contain(email0);
            customer.Emails.Should().Contain(email1);
            customer.Emails.Count.Should().Be(2);
            Email nullEmail = null;

            var result = customer.RemoveEmail(nullEmail);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void AddVehicles(Customer customer)
        {
            var honda = CreateHondaPilot();
            var pontiac = CreatePontiacTransSport();

            customer.AddVehicle(honda);
            customer.AddVehicle(pontiac);

            customer.Vehicles.Count.Should().Be(2);
            customer.Vehicles.Should().Contain(honda);
            customer.Vehicles.Should().Contain(pontiac);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Add_Null_Vehicle(Customer customer)
        {
            var honda = CreateHondaPilot();
            customer.AddVehicle(honda);

            var result = customer.AddVehicle(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Add_Duplicate_Vehicle(Customer customer)
        {
            var honda = CreateHondaPilot();
            customer.AddVehicle(honda);

            var resut = customer.AddVehicle(honda);

            resut.IsFailure.Should().BeTrue();
            resut.Error.Should().Contain(Customer.DuplicateItemMessagePrefix);
            customer.Vehicles.Should().Contain(honda);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void RemoveVehicle(Customer customer)
        {
            var honda = CreateHondaPilot();
            var pontiac = CreatePontiacTransSport();
            customer.AddVehicle(honda);
            customer.AddVehicle(pontiac);

            var result = customer.RemoveVehicle(honda);

            result.IsSuccess.Should().BeTrue();
            customer.Vehicles.Count.Should().Be(1);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Not_Remove_Null_Vehicle(Customer customer)
        {
            var honda = CreateHondaPilot();
            var pontiac = CreatePontiacTransSport();
            customer.AddVehicle(honda);
            customer.AddVehicle(pontiac);
            Vehicle nullVehicle = null;

            var result = customer.RemoveVehicle(nullVehicle);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Customer.RequiredMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.DataCustomer), MemberType = typeof(TestData))]
        public void Customer_Name_Is_Entity_Name(Customer customer)
        {
            customer.Name.Should().Be(customer.EntityType == EntityType.Person ? customer.Person.Name.FirstMiddleLast : customer.Organization.Name.Name);
        }

        private static Vehicle CreatePontiacTransSport()
        {
            var vin1 = "1GMDX03E8VD266902";
            var year1 = 2010;
            var make1 = "Pontiac";
            var model1 = "Trans Sport";
            var vehicle1 = Vehicle.Create(vin1, year1, make1, model1).Value;
            return vehicle1;
        }

        private static Vehicle CreateHondaPilot()
        {
            var vin0 = Utilities.LoremIpsum(17);
            var year0 = 2020;
            var make0 = "Honda";
            var model0 = "Pilot";
            var vehicle0 = Vehicle.Create(vin0, year0, make0, model0).Value;
            return vehicle0;
        }

        internal class TestData
        {
            public static IEnumerable<object[]> DataCustomer
            {
                get
                {
                    yield return new object[] { Customer.Create(ContactableTestHelper.CreatePerson(), CustomerType.Retail).Value
                };
                    yield return new object[] { Customer.Create(ContactableTestHelper.CreateOrganization(), CustomerType.Retail).Value };
                }
            }
        }
    }
}

