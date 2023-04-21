using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class EmailShould
    {
        [Fact]
        public void Create_Email()
        {
            var address = "john@doe.com";
            var primary = true;

            var emailFeeOrError = Email.Create(address, primary);

            emailFeeOrError.Value.Should().BeOfType<Email>();
            emailFeeOrError.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Return_Failure_Result_With_Null_Address()
        {
            string address = null;
            var primary = true;

            var result = Email.Create(address, primary);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Email.EmptyMessage);

        }

        [Fact]
        public void Return_Failure_Result_With_Empty_Address()
        {
            var address = string.Empty;
            var primary = true;

            var result = Email.Create(address, primary);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Email.EmptyMessage);
        }

        [Fact]
        public void Return_Failure_Result_With_Invalid_Address()
        {
            var address = "johnatdoedotcom";
            var primary = true;

            var result = Email.Create(address, primary);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(Email.InvalidMessage);
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

            email.SetAddress(newAddress);

            email.Address.Should().Be(newAddress);
        }

        [Fact]
        public void Update_Email_On_SetAddress()
        {
            var email = Create_Primary_Email();
            var newAddress = "new@address.com";

            email.SetAddress(newAddress);

            email.Address.Should().Be(newAddress);
        }

        [Fact]
        public void Update_Email_On_SetIsPrimary()
        {
            var email = Create_Primary_Email();

            email.IsPrimary.Should().BeTrue();
            email.SetIsPrimary(false);

            email.IsPrimary.Should().BeFalse();
        }

        internal static Email Create_Primary_Email()
        {
            var address = "john@doe.com";
            var primary = true;

            return Email.Create(address, primary).Value;
        }
    }
}