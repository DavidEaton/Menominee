using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class EmailShould
    {
        [Fact]
        public void Create_Valid_Email()
        {
            var address = "john@doe.com";
            var primary = true;

            var email = Email.Create(address, primary).Value;

            email.Should().BeOfType<Email>();
        }

        [Fact]
        public void Return_Failure_Result_With_Null_Address()
        {
            string address = null;
            var primary = true;

            var email = Email.Create(address, primary);

            email.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_Result_With_Empty_Address()
        {
            var address = string.Empty;
            var primary = true;

            var email = Email.Create(address, primary);

            email.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_Result_With_Malformatted_Address()
        {
            var address = "johnatdoedotcom";
            var primary = true;

            var email = Email.Create(address, primary);

            email.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Equate_Email_Instances_Having_Same_Values()
        {
            var address = "john@doe.com";
            var primary = true;

            var email1 = Email.Create(address, primary).Value;
            var email2 = Email.Create(address, primary).Value;

            email1.Should().BeEquivalentTo(email2);
        }

        [Fact]
        public void Not_Equate_Email_Instances_Having_Differing_Values()
        {
            var address = "john@doe.com";
            var primary = true;
            var email1 = Email.Create(address, primary).Value;
            var newAddress = "jane@doe.com";
            primary = false;

            var email2 = Email.Create(newAddress, primary).Value;

            email1.Should().NotBeEquivalentTo(email2);
        }

        [Fact]
        public void Return_Failure_Result_On_New_Address_With_Null_Address()
        {
            var email = Create_Primary_Email();
            var newAddress = "new@address.com";

            email = email.NewAddress(newAddress);

            email.Address.Should().Be(newAddress);
        }

        [Fact]
        public void Return_New_Email_On_New_Address()
        {
            var email = Create_Primary_Email();
            var newAddress = "new@address.com";

            email = email.NewAddress(newAddress);

            email.Address.Should().Be(newAddress);
        }

        [Fact]
        public void Return_New_Email_On_New_Primary()
        {
            var email = Create_Primary_Email();
            var newPrimary = false;

            email = email.NewPrimary(newPrimary);

            email.IsPrimary.Should().Be(newPrimary);
        }

        internal static Email Create_Primary_Email()
        {
            var address = "john@doe.com";
            var primary = true;

            return Email.Create(address, primary).Value;
        }
    }
}