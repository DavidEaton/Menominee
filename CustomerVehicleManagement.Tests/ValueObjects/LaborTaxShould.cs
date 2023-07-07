using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using FluentAssertions;
using Xunit;

namespace CustomerVehicleManagement.Tests.ValueObjects
{
    public class LaborTaxShould
    {
        [Fact]
        public void Create_LaborTax()
        {
            var result = LaborTax.Create(0.05, 100);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<LaborTax>();
            result.Value.Rate.Should().Be(0.05);
            result.Value.Amount.Should().Be(100);
        }

        [Fact]
        public void Return_Failure_On_Create_LaborTax_With_Negative_Rate()
        {
            var result = LaborTax.Create(-0.01, 100);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain(LaborTax.InvalidMessage);
        }

        [Fact]
        public void NewRate_Should_Update_Rate()
        {
            var initialRate = 10.0;
            var newRate = 15.0;
            var laborTax = LaborTax.Create(initialRate, 20.0).Value;
            laborTax.Rate.Should().Be(initialRate);

            var result = laborTax.NewRate(newRate);

            result.IsSuccess.Should().BeTrue();
            result.Value.Rate.Should().Be(newRate);
        }

        [Fact]
        public void Return_Failure_On_NewRate_With_Negative_Value()
        {
            var initialRate = .07;
            var newRate = -1;
            var laborTax = LaborTax.Create(initialRate, 20.0).Value;
            laborTax.Rate.Should().Be(initialRate);

            var result = laborTax.NewRate(newRate);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain(LaborTax.InvalidMessage);
        }

        [Fact]
        public void NewAmount_Should_Update_Amount()
        {
            var initialAmount = 10.0;
            var newAmount = 15.0;
            var laborTax = LaborTax.Create(20.0, initialAmount).Value;
            laborTax.Amount.Should().Be(initialAmount);

            var result = laborTax.NewAmount(newAmount);

            result.IsSuccess.Should().BeTrue();
            result.Value.Amount.Should().Be(newAmount);
        }

        [Fact]
        public void Return_True_If_Rate_And_Amount_Are_Equal()
        {
            var rate = 10.0;
            var amount = 20.0;
            var laborTax1 = LaborTax.Create(rate, amount).Value;
            var laborTax2 = LaborTax.Create(rate, amount).Value;

            var result = laborTax1.Equals(laborTax2);

            result.Should().BeTrue();
        }
    }
}
