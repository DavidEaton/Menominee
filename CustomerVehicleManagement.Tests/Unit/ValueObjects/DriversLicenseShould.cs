using FluentAssertions;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.ValueObjects
{
    public class DriversLicenseShould
    {
        [Fact]
        public void Create_New_DriversLicense()
        {
            var driversLicense = Create_DriversLicense();

            driversLicense.Should().NotBeNull();
        }

        [Fact]
        public void Return_IsFailure_Result_With_Empty_Number()
        {
            string driversLicenseNumber = null;
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            DateTimeRange driversLicenseValidRange = DateTimeRange.Create(issued, expiry).Value;

            var driversLicenseOrError = DriversLicense.Create(driversLicenseNumber, State.MI, driversLicenseValidRange);

            driversLicenseOrError.IsFailure.Should().BeTrue();
            driversLicenseOrError.Error.Should().Be(DriversLicense.RequiredMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_With_Empty_ValidRange()
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

            driversLicense2 = driversLicense2.NewNumber("BR549");

            driversLicense1.Should().NotBe(driversLicense2);
        }

        [Fact]
        public void Return_New_DriversLicense_On_NewNumber()
        {
            var driversLicense = Create_DriversLicense();
            var newNumber = "BR549";

            driversLicense = driversLicense.NewNumber(newNumber);

            driversLicense.Number.Should().Be(newNumber);
        }

        [Fact]
        public void Return_New_DriversLicense_On_NewState()
        {
            var driversLicense = Create_DriversLicense();

            var newState = State.CA;
            driversLicense = driversLicense.NewState(State.CA);

            driversLicense.State.Should().Be(newState);
        }

        [Fact]
        public void Return_New_DriversLicense_On_NewValidRange()
        {
            var driversLicense = Create_DriversLicense();
            var issued = DateTime.Today.AddYears(4);
            var expiry = DateTime.Today.AddYears(8);

            driversLicense = driversLicense.NewValidRange(issued, expiry);

            driversLicense.ValidDateRange.Start.Should().Be(issued);
            driversLicense.ValidDateRange.End.Should().Be(expiry);
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
