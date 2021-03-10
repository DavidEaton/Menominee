using FluentAssertions;
using SharedKernel.ValueObjects;
using System;
using Xunit;

namespace CustomerVehicleManagement.Tests.ValueObjectTests
{
    public class DateTimeRangeShould
    {
        [Fact]
        public void CreateDateTimeRange()
        {
            // Arrange
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);

            // Act
            var range = new DateTimeRange(start, end);

            // Assert
            range.Start.Should().BeCloseTo(start);
            range.End.Should().BeCloseTo(end);
        }

        [Fact]
        public void ThrowExceptionWhenEndPreceedsStart()
        {
            var end = DateTime.Today;
            var start = DateTime.Today.AddDays(1);

            Action action = () => new DateTimeRange(start, end);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(DateTimeRange.DateTimeRangeInvalidMessage);
        }

        [Fact]
        public void EquateTwoDateTimeRangeInstancesHavingSameValues()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);

            var range1 = new DateTimeRange(start, end);
            var range2 = new DateTimeRange(start, end);

            range1.Should().BeEquivalentTo(range2);
        }

        [Fact]
        public void NotEquateTwoDateTimeRangeInstancesHavingDifferingValues()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);
            var range1 = new DateTimeRange(start, end);
            var range2 = new DateTimeRange(start, end.AddDays(1));

            range1.Should().NotBeEquivalentTo(range2);
        }

        [Fact]
        public void ReturnNewDateTimeRangeOnNewStart()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);
            var range = new DateTimeRange(start, end);

            range = range.NewStart(DateTime.Today.AddDays(-10));

            range.Start.Should().Be(DateTime.Today.AddDays(-10));
            range.End.Should().Be(DateTime.Today.AddDays(1));
        }

        [Fact]
        public void ReturnNewDateTimeRangeOnNewEnd()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);
            var range = new DateTimeRange(start, end);

            range = range.NewEnd(DateTime.Today.AddDays(10));

            range.Start.Should().Be(DateTime.Today);
            range.End.Should().Be(DateTime.Today.AddDays(10));
        }

        [Fact]
        public void ReturnNewDateTimeRangeOnNewDuration()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(10);
            TimeSpan duration = new TimeSpan(TimeSpan.TicksPerDay);
            var range = new DateTimeRange(start, end);

            range = range.NewDuration(duration);

            range.Start.Should().Be(DateTime.Today);
            range.End.Should().Be(DateTime.Today.AddTicks(TimeSpan.TicksPerDay));
        }

        [Fact]
        public void CreateOneDayRange()
        {
            var range = DateTimeRange.CreateOneDayRange(DateTime.Today);

            range.Start.Should().Be(DateTime.Today);
            range.End.Should().Be(DateTime.Today.AddDays(1));
        }

        [Fact]
        public void CreateOneWeekRange()
        {
            var range = DateTimeRange.CreateOneWeekRange(DateTime.Today);

            range.Start.Should().Be(DateTime.Today);
            range.End.Should().Be(DateTime.Today.AddDays(7));
        }

        [Fact]
        public void CreateRangeWithDuration()
        {
            var duration = new TimeSpan(TimeSpan.TicksPerDay * 10);
            var range = new DateTimeRange(DateTime.Today, duration);

            range.Start.Should().Be(DateTime.Today);
            range.End.Should().Be(DateTime.Today.AddDays(10));
        }
    }
}
