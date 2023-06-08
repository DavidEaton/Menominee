using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class EmailShould
    {
        private const string InvalidStringOverMaximumLength = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in"; // 256 characters
        private const string InvalidStringZeroLength = "";
        private const string InvalidUnderMinimumLength = "1";

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

        [Theory]
        [InlineData(InvalidStringZeroLength)]
        [InlineData(InvalidUnderMinimumLength)]
        [InlineData(InvalidStringOverMaximumLength)]
        [InlineData("invalid-email-address.com")]
        public void Return_Failure_Result_With_Invalid_Address(string address)
        {
            var primary = true;

            var result = Email.Create(address, primary);

            result.IsFailure.Should().BeTrue();
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

        [Theory]
        [InlineData(InvalidStringZeroLength)]
        [InlineData(InvalidUnderMinimumLength)]
        [InlineData(InvalidStringOverMaximumLength)]
        [InlineData("invalid-email-address.com")]
        public void Not_SetAddress_Email_With_Invalid_Parameter(string address)
        {
            var email = Create_Primary_Email();

            var result = email.SetAddress(address);

            result.IsFailure.Should().BeTrue();
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