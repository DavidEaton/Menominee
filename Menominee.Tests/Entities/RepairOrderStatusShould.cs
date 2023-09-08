using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using System;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderStatusShould
    {
        [Theory]
        [InlineData(Status.New, "Valid description")]
        [InlineData(Status.EstimateQuote, "Valid description")]
        [InlineData(Status.Approved, "Valid description")]
        [InlineData(Status.InProgress, "Valid description")]
        [InlineData(Status.OnHold, "Valid description")]
        [InlineData(Status.Invoiced, "Valid description")]
        [InlineData(Status.AwaitingPayment, "Valid description")]
        [InlineData(Status.Completed, "Valid description")]
        [InlineData(Status.PickedUp, "Valid description")]
        public void Create_RepairOrderStatus(Status status, string description)
        {
            var result = RepairOrderStatus.Create(status, description);

            result.IsSuccess.Should().BeTrue();
            result.Value.Status.Should().Be(status);
            result.Value.Description.Should().Be(description);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Invalid_Status()
        {
            var result = RepairOrderStatus.Create((Status)999, "Valid description");

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderStatus.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Invalid_Description()
        {
            var result = RepairOrderStatus.Create(Status.New, new string('a', RepairOrderStatus.MaximumLength + 1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderStatus.InvalidLengthMessage);
        }

        [Fact]
        public void SetDate()
        {
            var status = RepairOrderStatus.Create(Status.New, "Valid description").Value;
            var result = status.SetDate(DateTime.Today);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(DateTime.Today);
        }

        [Fact]
        public void Return_Failure_On_Set_Future_Date()
        {
            var status = RepairOrderStatus.Create(Status.New, "Valid description").Value;
            var result = status.SetDate(DateTime.Today.AddDays(1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderStatus.InvalidDateMessage);
        }

        [Theory]
        [InlineData(Status.New)]
        [InlineData(Status.EstimateQuote)]
        [InlineData(Status.Approved)]
        [InlineData(Status.InProgress)]
        [InlineData(Status.OnHold)]
        [InlineData(Status.Invoiced)]
        [InlineData(Status.AwaitingPayment)]
        [InlineData(Status.Completed)]
        [InlineData(Status.PickedUp)]
        public void SetStatus(Status newStatus)
        {
            var status = RepairOrderStatus.Create(Status.New, "Valid description").Value;
            var result = status.SetStatus(newStatus);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(newStatus);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_Status()
        {
            var status = RepairOrderStatus.Create(Status.New, "Valid description").Value;
            var result = status.SetStatus((Status)999);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderStatus.RequiredMessage);
        }

        [Fact]
        public void SetDescription()
        {
            var status = RepairOrderStatus.Create(Status.New, "Valid description").Value;
            var result = status.SetDescription("Another valid description");

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be("Another valid description");
        }
    }
}
