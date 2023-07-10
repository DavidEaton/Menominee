using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class DriversLicenseShould
    {
        private const string InvalidStringOverMaximumLength = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in"; // 256 characters
        private const string InvalidStringZeroLength = "";
        private const string InvalidUnderMinimumLength = "1";

        [Fact]
        public void Create_New_DriversLicense()
        {
            var driversLicense = Create_DriversLicense();

            driversLicense.Should().NotBeNull();
        }

        [Theory]
        [InlineData(InvalidUnderMinimumLength)]
        [InlineData(InvalidStringOverMaximumLength)]
        public void Return_IsFailure_Result_On_Create_With_Invalid_Number(string driversLicenseNumber)
        {
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            DateTimeRange driversLicenseValidRange = DateTimeRange.Create(issued, expiry).Value;

            var driversLicenseOrError = DriversLicense.Create(driversLicenseNumber, State.MI, driversLicenseValidRange);

            driversLicenseOrError.IsFailure.Should().BeTrue();
            driversLicenseOrError.Error.Should().Contain("length");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(InvalidStringZeroLength)]
        public void Return_IsFailure_Result_On_Create_With_Empty_Number(string driversLicenseNumber)
        {
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            DateTimeRange driversLicenseValidRange = DateTimeRange.Create(issued, expiry).Value;

            var driversLicenseOrError = DriversLicense.Create(driversLicenseNumber, State.MI, driversLicenseValidRange);

            driversLicenseOrError.IsFailure.Should().BeTrue();
            driversLicenseOrError.Error.Should().Be(DriversLicense.RequiredMessage);
        }
        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Invalid_State()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            DateTimeRange driversLicenseValidRange = DateTimeRange.Create(issued, expiry).Value;
            State invalidState = (State)(-1);

            var result = DriversLicense.Create(driversLicenseNumber, invalidState, driversLicenseValidRange);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(DriversLicense.StateInvalidMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_DateRange()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            DateTimeRange driversLicenseValidRange = null;

            var driversLicenseOrError = DriversLicense.Create(driversLicenseNumber, State.MI, driversLicenseValidRange);

            driversLicenseOrError.IsFailure.Should().BeTrue();
            driversLicenseOrError.Error.Should().Be(DriversLicense.DateRangeInvalidMessage);
        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var driversLicense1 = Create_DriversLicense();
            var driversLicense2 = Create_DriversLicense();

            driversLicense1.Should().Be(driversLicense2);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var driversLicense1 = Create_DriversLicense();
            var driversLicense2 = Create_DriversLicense();

            driversLicense2 = driversLicense2.NewNumber("BR549").Value;

            driversLicense1.Should().NotBe(driversLicense2);
        }

        [Fact]
        public void Return_New_DriversLicense_On_NewNumber()
        {
            var driversLicense = Create_DriversLicense();
            var newNumber = "BR549";

            var result = driversLicense.NewNumber(newNumber);

            result.Value.Number.Should().Be(newNumber);
        }

        [Fact]
        public void Return_New_DriversLicense_On_NewState()
        {
            var driversLicense = Create_DriversLicense();

            var newState = State.CA;
            var result = driversLicense.NewState(State.CA);

            result.Value.State.Should().Be(newState);
        }

        [Fact]
        public void Return_New_DriversLicense_On_NewValidRange()
        {
            var driversLicense = Create_DriversLicense();
            var issued = DateTime.Today.AddYears(4);
            var expiry = DateTime.Today.AddYears(8);

            var result = driversLicense.NewValidRange(issued, expiry);

            result.Value.ValidDateRange.Start.Should().Be(issued);
            result.Value.ValidDateRange.End.Should().Be(expiry);
        }

        internal static DriversLicense Create_DriversLicense()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            var driversLicenseValidRange = DateTimeRange.Create(issued, expiry).Value;

            return DriversLicense.Create(driversLicenseNumber, State.MI, driversLicenseValidRange).Value;
        }
    }

}
