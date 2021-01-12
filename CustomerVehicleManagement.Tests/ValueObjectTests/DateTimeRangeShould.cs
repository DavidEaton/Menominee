using NUnit.Framework;
using SharedKernel.ValueObjects;
using System;

namespace CustomerVehicleManagement.Tests.ValueObjectTests
{
    public class DateTimeRangeShould
    {
        [Test]
        public void CreateValidDateTimeRange()
        {
            // Arrange
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(1);

            // Act
            var range = new DateTimeRange(start, end);
            var minutes = range.DurationInMinutes();

            // Assert
            Assert.That(minutes, Is.EqualTo(0));
        }

        [Test]
        public void ThrowExceptionWhenEndPreceedsStart()
        {
            DateTime end = DateTime.Today;
            DateTime start = DateTime.Today.AddDays(1);

            var exception = Assert.Throws<ArgumentException>(
                () => { new DateTimeRange(start, end); });

            Assert.That(exception.Message, Is.EqualTo(DateTimeRange.DateTimeRangeInvalidMessage));
        }

        [Test]
        public void EquateTwoDateTimeRangeInstancesHavingSameValues()
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(1);

            var range1 = new DateTimeRange(start, end);
            var range2 = new DateTimeRange(start, end);

            Assert.That(range1.Equals(range2));
        }

        [Test]
        public void NotEquateTwoDateTimeRangeInstancesHavingDifferingValues()
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(1);

            var range1 = new DateTimeRange(start, end);
            var range2 = new DateTimeRange(start, end);

            range2 = range2.NewStart(DateTime.Today.AddDays(-20));

            Assert.That(range1, Is.Not.EqualTo(range2));
        }

        [Test]
        public void ReturnNewDateTimeRangeOnNewStart()
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(1);

            var range = new DateTimeRange(start, end);
            range = range.NewStart(DateTime.Today.AddDays(-10));

            Assert.That(range.Start, Is.EqualTo(DateTime.Today.AddDays(-10)));
            Assert.That(range.End, Is.EqualTo(DateTime.Today.AddDays(1)));
        }

        [Test]
        public void ReturnNewDateTimeRangeOnNewEnd()
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(1);

            var range = new DateTimeRange(start, end);
            range = range.NewEnd(DateTime.Today.AddDays(10));

            Assert.That(range.Start, Is.EqualTo(DateTime.Today));
            Assert.That(range.End, Is.EqualTo(DateTime.Today.AddDays(10)));
        }

        [Test]
        public void ReturnNewDateTimeRangeOnNewDuration()
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Today.AddDays(10);
            TimeSpan duration = new TimeSpan(TimeSpan.TicksPerDay);

            var range = new DateTimeRange(start, end);
            range = range.NewDuration(duration);

            Assert.That(range.Start, Is.EqualTo(DateTime.Today));
            Assert.That(range.End, Is.EqualTo(DateTime.Today.AddTicks(TimeSpan.TicksPerDay)));
        }

        [Test]
        public void CreateOneDayRange()
        {
            var range = DateTimeRange.CreateOneDayRange(DateTime.Today);

            Assert.That(range.Start, Is.EqualTo(DateTime.Today));
            Assert.That(range.End, Is.EqualTo(DateTime.Today.AddDays(1)));
        }

        [Test]
        public void CreateOneWeekRange()
        {
            var range = DateTimeRange.CreateOneWeekRange(DateTime.Today);

            Assert.That(range.Start, Is.EqualTo(DateTime.Today));
            Assert.That(range.End, Is.EqualTo(DateTime.Today.AddDays(7)));
        }

        [Test]
        public void CreateRangeWithDuration()
        {
            TimeSpan duration = new TimeSpan(TimeSpan.TicksPerDay * 10);
            var range = new DateTimeRange(DateTime.Today, duration);

            Assert.That(range.Start, Is.EqualTo(DateTime.Today));
            Assert.That(range.End, Is.EqualTo(DateTime.Today.AddDays(10)));
        }

    }
}
