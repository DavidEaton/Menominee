using Migrations.Core.ValueObjects;
using NUnit.Framework;
using SharedKernel.ValueObjects;
using System;

namespace Migrations.Tests.ValueObjectTests
{
    public class DriversLicenseShould
    {
        [Test]
        public void CreateNewDriversLicense()
        {
            var driversLicense = CreateDriversLicense();

            Assert.That(driversLicense, Is.Not.Null);
        }

        [Test]
        public void ThrowExceptionWithEmptyDriversLicenseDetails()
        {
            string driversLicenseNumber = null;
            string driversLicenseState = "MI";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);

            DateTimeRange driversLicenseValidRange = new DateTimeRange(issued, expiry);

            var exception = Assert.Throws<ArgumentException>(
                () => { new DriversLicence(driversLicenseNumber, driversLicenseState, driversLicenseValidRange); });

            Assert.That(exception.Message, Is.EqualTo(DriversLicence.DriversLicenceInvalidMessage));

            driversLicenseNumber = "123456789POIUYTREWQ";
            driversLicenseState = null;
            issued = DateTime.Today;
            expiry = DateTime.Today.AddYears(4);

            driversLicenseValidRange = new DateTimeRange(issued, expiry);

            exception = Assert.Throws<ArgumentException>(
                () => { new DriversLicence(driversLicenseNumber, driversLicenseState, driversLicenseValidRange); });

            Assert.That(exception.Message, Is.EqualTo(DriversLicence.DriversLicenceInvalidMessage));

            driversLicenseNumber = "123456789POIUYTREWQ";
            driversLicenseState = "MI";
            driversLicenseValidRange = null;

            exception = Assert.Throws<ArgumentException>(
                () => { new DriversLicence(driversLicenseNumber, driversLicenseState, driversLicenseValidRange); });

            Assert.That(exception.Message, Is.EqualTo(DriversLicence.DriversLicenceInvalidMessage));
        }

        [Test]
        public void EquateTwoDriversLicenseInstancesHavingSameValues()
        {
            var driversLicense1 = CreateDriversLicense();
            var driversLicense2 = CreateDriversLicense();

            Assert.That(driversLicense1.Equals(driversLicense2));
        }

        [Test]
        public void NotEquateTwoDriversLicenseInstancesHavingDifferingValues()
        {
            var driversLicense1 = CreateDriversLicense();
            var driversLicense2 = CreateDriversLicense();

            driversLicense2 = driversLicense2.NewNumber("BR549");

            Assert.That(driversLicense1, Is.Not.EqualTo(driversLicense2));
        }

        [Test]
        public void ReturnNewDriversLicenseOnNewLicenseNumber()
        {
            var driversLicense = CreateDriversLicense();

            driversLicense = driversLicense.NewNumber("BR549");

            Assert.That(driversLicense.Number, Is.EqualTo("BR549"));
        }

        [Test]
        public void ReturnNewDriversLicenseOnNewLicenseState()
        {
            var driversLicense = CreateDriversLicense();

            driversLicense = driversLicense.NewState("CO");

            Assert.That(driversLicense.State, Is.EqualTo("CO"));
        }

        [Test]
        public void ReturnNewDriversLicenseOnNewLicenseValidRange()
        {
            var driversLicense = CreateDriversLicense();
            var issued = DateTime.Today.AddYears(4);
            var expiry = DateTime.Today.AddYears(8);

            driversLicense = driversLicense.NewValidRange(issued, expiry);

            Assert.That(driversLicense.ValidRange.Start, Is.EqualTo(issued));
            Assert.That(driversLicense.ValidRange.End, Is.EqualTo(expiry));
        }

        internal static DriversLicence CreateDriversLicense()
        {
            var driversLicenseNumber = "123456789POIUYTREWQ";
            var driversLicenseState = "MI";
            var issued = DateTime.Today;
            var expiry = DateTime.Today.AddYears(4);
            var driversLicenseValidRange = new DateTimeRange(issued, expiry);

            return new DriversLicence(driversLicenseNumber, driversLicenseState, driversLicenseValidRange);
        }
    }
}
