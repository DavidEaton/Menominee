using Bogus;
using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using System;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderTaxShould
    {
        [Fact]
        public void Create_RepairOrderTax()
        {
            var faker = new Faker();
            var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);

            var result = RepairOrderTax.Create(
                PartTax.Create(rate, amount).Value,
                LaborTax.Create(rate, amount).Value);

            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderTax_With_Null_PartTax()
        {
            var faker = new Faker();
            var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);

            var result = RepairOrderTax.Create(
                null,
                LaborTax.Create(rate, amount).Value);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderTax_With_Null_LaborTax()
        {
            var faker = new Faker();
            var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);

            var result = RepairOrderTax.Create(
                PartTax.Create(rate, amount).Value,
                null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderTax_With_Null_Taxes()
        {
            var result = RepairOrderTax.Create(null, null);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetPartTax()
        {
            var faker = new Faker();
            var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var repairOrderTax = RepairOrderTax.Create(
                PartTax.Create(rate, amount).Value,
                LaborTax.Create(rate, amount).Value).Value;
            var newPartTax = PartTax.Create(rate, amount).Value;

            var result = repairOrderTax.SetPartTax(newPartTax);

            result.IsSuccess.Should().BeTrue();
            repairOrderTax.PartTax.Should().Be(newPartTax);
        }

        [Fact]
        public void Return_Failure_On_Set_Null_PartTax()
        {
            var faker = new Faker();
            var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var repairOrderTax = RepairOrderTax.Create(
                PartTax.Create(rate, amount).Value,
                LaborTax.Create(rate, amount).Value).Value;

            var result = repairOrderTax.SetPartTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderTax.RequiredMessage);
        }

        [Fact]
        public void SetLaborTax()
        {
            var faker = new Faker();
            var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var repairOrderTax = RepairOrderTax.Create(
                PartTax.Create(rate, amount).Value,
                LaborTax.Create(rate, amount).Value).Value;
            var newLaborTax = LaborTax.Create(rate, amount).Value;

            var result = repairOrderTax.SetLaborTax(newLaborTax);

            result.IsSuccess.Should().BeTrue();
            repairOrderTax.LaborTax.Should().Be(newLaborTax);
        }

        [Fact]
        public void Return_Failure_On_Set_Nul_LaborTax()
        {
            var faker = new Faker();
            var rate = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var amount = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var repairOrderTax = RepairOrderTax.Create(
                PartTax.Create(rate, amount).Value,
                LaborTax.Create(rate, amount).Value).Value;

            var result = repairOrderTax.SetLaborTax(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderTax.RequiredMessage);
        }
    }
}
