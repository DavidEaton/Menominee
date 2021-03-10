using FluentAssertions;
using SharedKernel.ValueObjects;
using System;
using Xunit;

namespace CustomerVehicleManagement.Tests.ValueObjectTests
{
    public class DriversLicenseShould
    {
        [Fact]
        public void CreateNewDriversLicense()
        {
            var driversLicense = CreateDriversLicense();

            driversLicense.Should().NotBeNull();
        }

        [Fact]
        public void ThrowExceptionWithEmptyNumber()
        {
            string driversLicenseNumber = null;
            var driversLicenseState = "MI";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            DateTimeRange driversLicenseValidRange = new DateTimeRange(issued, expiry);

            var exception = Assert.Throws<ArgumentException>(
                () => { new DriversLicense(driversLicenseNumber, driversLicenseState, driversLicenseValidRange); });

            Action action = () => new DriversLicense(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(DriversLicense.DriversLicenseInvalidMessage);
        }

        [Fact]
        public void ThrowExceptionWithEmptyState()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            string driversLicenseState = null;
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);

            DateTimeRange driversLicenseValidRange = new DateTimeRange(issued, expiry);

            Action action = () => new DriversLicense(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(DriversLicense.DriversLicenseInvalidMessage);
        }

        [Fact]
        public void ThrowExceptionWithEmptyValidRange()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var driversLicenseState = "MI";

            DateTimeRange driversLicenseValidRange = null;

            Action action = () => new DriversLicense(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(DriversLicense.DriversLicenseInvalidMessage);
        }

        [Fact]
        public void EquateTwoDriversLicenseInstancesHavingSameValues()
        {
            var driversLicense1 = CreateDriversLicense();
            var driversLicense2 = CreateDriversLicense();

            driversLicense1.Should().Be(driversLicense2);
        }

        [Fact]
        public void NotEquateTwoDriversLicenseInstancesHavingDifferingValues()
        {
            var driversLicense1 = CreateDriversLicense();
            var driversLicense2 = CreateDriversLicense();

            driversLicense2 = driversLicense2.NewNumber("BR549");

            driversLicense1.Should().NotBe(driversLicense2);
        }

        [Fact]
        public void ReturnNewDriversLicenseOnNewLicenseNumber()
        {
            var driversLicense = CreateDriversLicense();
            var newNumber = "BR549";

            driversLicense = driversLicense.NewNumber(newNumber);

            driversLicense.Number.Should().Be(newNumber);
        }

        [Fact]
        public void ReturnNewDriversLicenseOnNewLicenseState()
        {
            var driversLicense = CreateDriversLicense();
            var newState = "CO";

            driversLicense = driversLicense.NewState(newState);

            driversLicense.State.Should().Be(newState);
        }

        [Fact]
        public void ReturnNewDriversLicenseOnNewLicenseValidRange()
        {
            var driversLicense = CreateDriversLicense();
            var issued = DateTime.Today.AddYears(4);
            var expiry = DateTime.Today.AddYears(8);

            driversLicense = driversLicense.NewValidRange(issued, expiry);

            driversLicense.ValidRange.Start.Should().Be(issued);
            driversLicense.ValidRange.End.Should().Be(expiry);
        }

        internal static DriversLicense CreateDriversLicense()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var driversLicenseState = "MI";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            var driversLicenseValidRange = new DateTimeRange(issued, expiry);

            return new DriversLicense(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);
        }
    }

}
