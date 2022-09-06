using CustomerVehicleManagement.Domain.Entities.Taxes;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
{
    public class ExciseFeeShould
    {
        [Fact]
        public void Create_ExciseFee()
        {
            // Arrange
            string description = Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength - 1);
            ExciseFeeType feeType = ExciseFeeType.Flat;
            double amount = ExciseFee.MinimumValue + 1;

            // Act
            var exciseFeeOrError = ExciseFee.Create(description, feeType, amount);

            // Assert
            exciseFeeOrError.IsFailure.Should().BeFalse();
            exciseFeeOrError.Value.Should().BeOfType<ExciseFee>();
        }

        [Fact]
        public void Not_Create_ExciseFee_With_Invalid_Description()
        {
            string description = Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength + 1);
            ExciseFeeType feeType = ExciseFeeType.Flat;
            double amount = ExciseFee.MinimumValue + 1;

            var exciseFeeOrError = ExciseFee.Create(description, feeType, amount);

            exciseFeeOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_ExciseFee_With_Null_Description()
        {
            string nullDescription = null;
            ExciseFeeType feeType = ExciseFeeType.Flat;
            double amount = ExciseFee.MinimumValue + 1;

            var exciseFeeOrError = ExciseFee.Create(nullDescription, feeType, amount);

            exciseFeeOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_ExciseFee_With_Invalid_FeeType()
        {
            string description = Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength);
            var invalidExciseFeeType = (ExciseFeeType)(-1);
            double amount = ExciseFee.MinimumValue + 1;

            var exciseFeeOrError = ExciseFee.Create(description, invalidExciseFeeType, amount);

            exciseFeeOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_ExciseFee_With_Invalid_Amount(double invalidAmount)
        {
            string description = Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength);
            ExciseFeeType feeType = ExciseFeeType.Flat;

            var exciseFeeOrError = ExciseFee.Create(description, feeType, invalidAmount);

            exciseFeeOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetDescription()
        {
            var exciseFee = CreateExciseFee();

            var description = Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength - 1);
            exciseFee.SetDescription(description);

            exciseFee.Description.Should().Be(description);
        }

        [Fact]
        public void Not_Set_Null_Description()
        {
            var exciseFee = CreateExciseFee();
            string nullDescription = null;

            Action action = () => exciseFee.SetDescription(nullDescription);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Not_Set_Invalid_Description()
        {
            var exciseFee = CreateExciseFee();
            var invalidDescription = Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength + 1);

            Action action = () => exciseFee.SetDescription(invalidDescription);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void SetFeeType()
        {
            var exciseFee = CreateExciseFee();

            exciseFee.FeeType.Should().Be(ExciseFeeType.Flat);
            exciseFee.SetFeeType(ExciseFeeType.Percentage);

            exciseFee.FeeType.Should().Be(ExciseFeeType.Percentage);
        }

        [Fact]
        public void Not_Set_Invalid_FeeType()
        {
            var exciseFee = CreateExciseFee();
            var invalidFeeType = (ExciseFeeType)(-1);

            Action action = () => exciseFee.SetFeeType(invalidFeeType);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void SetAmount()
        {
            var exciseFee = CreateExciseFee();

            exciseFee.Amount.Should().Be(ExciseFee.MinimumValue + 1.0);
            exciseFee.SetAmount(ExciseFee.MinimumValue + 11.1);

            exciseFee.Amount.Should().Be(ExciseFee.MinimumValue + 11.1);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Amount(double invalidAmount)
        {
            var exciseFee = CreateExciseFee();

            Action action = () => exciseFee.SetAmount(invalidAmount);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        private ExciseFee CreateExciseFee()
        {
            return ExciseFee.Create(
                Utilities.RandomCharacters(ExciseFee.DescriptionMaximumLength - 1),
                ExciseFeeType.Flat,
                ExciseFee.MinimumValue + 1.0).Value;
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { ExciseFee.MaximumValue + .01 };
                    yield return new object[] { ExciseFee.MinimumValue - .01 };
                }
            }
        }
    }
}
