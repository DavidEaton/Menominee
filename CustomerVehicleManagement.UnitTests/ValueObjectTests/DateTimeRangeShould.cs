using FluentAssertions;
using FluentAssertions.Extensions;
using Menominee.Common.ValueObjects;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.ValueObjectTests
{
    public class DateTimeRangeShould
    {
        [Fact]
        public void Create_DateTimeRange()
        {
            // Arrange
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);

            // Act
            var range = DateTimeRange.Create(start, end).Value;

            // Assert
            range.Start.Should().BeCloseTo(start, 1.Minutes());
            range.End.Should().BeCloseTo(end, 1.Minutes());
        }

        [Fact]
        public void Return_IsFailure_Result_When_End_Preceeds_Start()
        {
            var end = DateTime.Today;
            var start = DateTime.Today.AddDays(1);

            var dateRangeOrError = DateTimeRange.Create(start, end);

            dateRangeOrError.IsFailure.Should().BeTrue();
            dateRangeOrError.Error.Should().Be(DateTimeRange.DateTimeRangeInvalidMessage);
        }

        [Fact]
        public void Equate_Two_DateTimeRange_Instances_Having_Same_Values()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);

            var range1 = DateTimeRange.Create(start, end).Value;
            var range2 = DateTimeRange.Create(start, end).Value;

            range1.Should().BeEquivalentTo(range2);
        }

        [Fact]
        public void Not_Equate_Two_DateTimeRange_Instances_Having_Differing_Values()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);
            var range1 = DateTimeRange.Create(start, end).Value;
            var range2 = DateTimeRange.Create(start, end.AddDays(1)).Value;

            range1.Should().NotBeEquivalentTo(range2);
        }

        [Fact]
        public void Return_New_DateTimeRange_On_NewStart()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);
            var range = DateTimeRange.Create(start, end).Value;

            range = range.NewStart(DateTime.Today.AddDays(-10));

            range.Start.Should().Be(DateTime.Today.AddDays(-10));
            range.End.Should().Be(DateTime.Today.AddDays(1));
        }

        [Fact]
        public void Return_IsFailure_Result_On_NewStart_Is_After_End()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);
            var range = DateTimeRange.Create(start, end).Value;

            Action action = () => range = range.NewStart(end.AddDays(1));

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Return_New_DateTimeRange_On_NewEnd()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);
            var range = DateTimeRange.Create(start, end).Value;

            range = range.NewEnd(DateTime.Today.AddDays(10));

            range.Start.Should().Be(DateTime.Today);
            range.End.Should().Be(DateTime.Today.AddDays(10));
        }

        [Fact]
        public void Return_IsFailure_Result_On_NewEnd_Is_Before_Start()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(1);
            var range = DateTimeRange.Create(start, end).Value;

            Action action = () => range = range.NewEnd(start.AddDays(-1));

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Return_New_DateTimeRange_On_NewDuration()
        {
            var start = DateTime.Today;
            var end = DateTime.Today.AddDays(10);
            TimeSpan duration = new TimeSpan(TimeSpan.TicksPerDay);
            var range = DateTimeRange.Create(start, end).Value;

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
        public void Create_DateTimeRange_With_Duration()
        {
            var duration = new TimeSpan(TimeSpan.TicksPerDay * 10);
            var range = new DateTimeRange(DateTime.Today, duration);

            range.Start.Should().Be(DateTime.Today);
            range.End.Should().Be(DateTime.Today.AddDays(10));
        }
    }
}
