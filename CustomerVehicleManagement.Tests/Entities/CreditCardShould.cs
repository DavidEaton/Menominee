using CustomerVehicleManagement.Domain.Entities;
using FluentAssertions;
using Menominee.Common.Enums;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class CreditCardShould
    {
        [Theory]
        [InlineData("Visa", CreditCardFeeType.Flat, 1.5, true, true)]
        [InlineData("Mastercard", CreditCardFeeType.Percentage, 0.05, false, true)]
        [InlineData("Discover", CreditCardFeeType.Percentage, 0.03, true, true)]
        public void Successfully_Create_Given_Valid_Inputs(
            string name,
            CreditCardFeeType feeType,
            double fee,
            bool? isAddedToDeposit,
            bool expectedResult)
        {
            // Act
            var result = CreditCard.Create(name, feeType, fee, isAddedToDeposit);

            // Assert
            result.IsSuccess.Should().Be(expectedResult);
            if (expectedResult)
            {
                result.Value.Should().NotBeNull();
                result.Value.Name.Should().Be(name);
                result.Value.FeeType.Should().Be(feeType);
                result.Value.Fee.Should().Be(fee);
                result.Value.IsAddedToDeposit.Should().Be(isAddedToDeposit);
            }
        }

        [Theory]
        [InlineData(null, CreditCardFeeType.Flat, 1.5, true, CreditCard.RequiredMessage)]
        [InlineData("", CreditCardFeeType.Percentage, 0.05, false, CreditCard.RequiredMessage)]
        [InlineData("Invalid", (CreditCardFeeType)(-1), 0.03, true, CreditCard.RequiredMessage)]
        [InlineData("m", CreditCardFeeType.Flat, 0.03, true, CreditCard.InvalidLengthMessage)]
        [InlineData("Loren ipsun dolor sit anet, consectetur adipisci el", CreditCardFeeType.Flat, 0.03, true, CreditCard.InvalidLengthMessage)]
        public void Fail_Create_Given_Invalid_Inputs(
            string name,
            CreditCardFeeType feeType,
            double fee,
            bool? isAddedToDeposit,
            string expectedMessage)
        {
            // Act
            var result = CreditCard.Create(name, feeType, fee, isAddedToDeposit);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(expectedMessage);
        }

        [Fact]
        public void Successfully_SetName_With_Valid_Name()
        {
            var creditCard = CreditCard.Create("Valid Name", CreditCardFeeType.Flat, 0.0, null)
                .Value;

            var result = creditCard.SetName("New Name");

            result.IsSuccess.Should().BeTrue();
            creditCard.Name.Should().Be("New Name");
        }

        [Theory]
        [InlineData(null, CreditCard.RequiredMessage)]
        [InlineData("", CreditCard.RequiredMessage)]
        [InlineData("  ", CreditCard.RequiredMessage)]
        [InlineData("N", CreditCard.InvalidLengthMessage)]
        [InlineData("Loren ipsun dolor sit anet, consectetur adipisci el", CreditCard.InvalidLengthMessage)]
        public void Fail_SetName_With_Invalid_Name(string name, string expectedMessage)
        {
            var creditCard = CreditCard.Create("Valid Name", CreditCardFeeType.Flat, 0.0, null)
                .Value;

            var result = creditCard.SetName(name);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(expectedMessage);
        }

        [Fact]
        public void SetFeeType_With_Valid_FeeType()
        {
            var creditCard = CreditCard.Create("Valid Name", CreditCardFeeType.Flat, 0.0, null)
                .Value;

            var result = creditCard.SetFeeType(CreditCardFeeType.Percentage);

            result.IsSuccess.Should().BeTrue();
            creditCard.FeeType.Should().Be(CreditCardFeeType.Percentage);
        }

        [Fact]
        public void Fail_SetFeeType_With_Invalid_FeeType()
        {
            var creditCard = CreditCard.Create("Valid Name", CreditCardFeeType.Flat, 0.0, null)
                .Value;

            var result = creditCard.SetFeeType((CreditCardFeeType)99);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Invalid Fee Type");
        }

        [Fact]
        public void SetFee_With_Valid_Fee()
        {
            var creditCard = CreditCard.Create("Valid Name", CreditCardFeeType.Flat, 0.0, null)
                .Value;

            var result = creditCard.SetFee(99.99);

            result.IsSuccess.Should().BeTrue();
            creditCard.Fee.Should().Be(99.99);
        }

        [Fact]
        public void Fail_SetFee_With_Invalid_Fee()
        {
            var creditCard = CreditCard.Create("Valid Name", CreditCardFeeType.Flat, 0.0, null)
                .Value;

            var result = creditCard.SetFee(99.99);

            result.IsSuccess.Should().BeTrue();
            creditCard.Fee.Should().Be(99.99);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void SetIsAddedToDeposit_With_Valid_Input(bool? originalIsAddedToDeposit, bool? newIsAddedToDeposit)
        {
            // Arrange
            var creditCard = CreditCard.Create("Visa", CreditCardFeeType.Percentage, 2.5, originalIsAddedToDeposit)
                .Value;

            // Act
            var result = creditCard.SetIsAddedToDeposit(newIsAddedToDeposit);

            // Assert
            result.IsSuccess.Should().BeTrue();
            creditCard.IsAddedToDeposit.Should().Be(newIsAddedToDeposit);
        }
    }
}
