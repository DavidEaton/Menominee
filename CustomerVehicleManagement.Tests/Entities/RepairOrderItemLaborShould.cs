using Bogus;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class RepairOrderItemLaborShould
    {
        private readonly Faker faker;

        public RepairOrderItemLaborShould()
        {
            faker = new Faker();
        }

        [Fact]
        public void Create_With_Valid_Input()
        {
            var techAmount = TechAmount.Create(
                faker.PickRandom<ItemLaborType>(),
                (double)Math.Round(faker.Random.Decimal(1, 1000), 2), SkillLevel.A)
                .Value;
            var laborAmount = LaborAmount.Create(
                faker.PickRandom<ItemLaborType>(),
                (double)Math.Round(faker.Random.Decimal(1, 1000), 2))
                .Value;

            var result = RepairOrderItemLabor.Create(laborAmount, techAmount);

            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Return_Failure_On_Create_With_Invalid_Input(LaborAmount laborAmount, TechAmount techAmount)
        {
            var result = RepairOrderItemLabor.Create(laborAmount, techAmount);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemLabor.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_SetLaborAmount_With_Null_LaborAmount()
        {
            var repairOrderItemLabor = new RepairOrderItemLaborFaker(true).Generate();

            var result = repairOrderItemLabor.SetLaborAmount(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemLabor.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_On_SetTechAmount_With_Null_TechAmount()
        {
            var repairOrderItemLabor = new RepairOrderItemLaborFaker(true).Generate();

            var result = repairOrderItemLabor.SetTechAmount(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemLabor.RequiredMessage);
        }

        [Fact]
        public void SetLaborAmount()
        {
            var faker = new Faker();
            var repairOrderItemLabor = new RepairOrderItemLaborFaker(true).Generate();

            var newLaborAmount = LaborAmount.Create(
                faker.PickRandom<ItemLaborType>(),
                (double)Math.Round(faker.Random.Decimal(1, 1000), 2))
                .Value;

            while (repairOrderItemLabor.LaborAmount == newLaborAmount)
            {
                newLaborAmount = LaborAmount.Create(
                    faker.PickRandom<ItemLaborType>(),
                    (double)Math.Round(faker.Random.Decimal(1, 1000), 2))
                    .Value;
            }

            repairOrderItemLabor.LaborAmount.Should().NotBe(newLaborAmount);

            var result = repairOrderItemLabor.SetLaborAmount(newLaborAmount);

            result.IsSuccess.Should().BeTrue();
            repairOrderItemLabor.LaborAmount.Should().Be(newLaborAmount);
        }

        [Fact]
        public void SetTechAmount()
        {
            var repairOrderItemLabor = new RepairOrderItemLaborFaker(true).Generate();

            var newTechAmount = TechAmount.Create(
                faker.PickRandom<ItemLaborType>(),
                (double)Math.Round(faker.Random.Decimal(1, 1000), 2), SkillLevel.A)
                .Value;
            repairOrderItemLabor.TechAmount.Should().NotBe(newTechAmount);

            var result = repairOrderItemLabor.SetTechAmount(newTechAmount);

            result.IsSuccess.Should().BeTrue();
            repairOrderItemLabor.TechAmount.Should().Be(newTechAmount);
        }

        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { null, TechAmount.Create(ItemLaborType.Flat, 10, SkillLevel.A).Value },
                new object[] { LaborAmount.Create(ItemLaborType.Flat, 10).Value, null },
                new object[] { null, null }
            };
    }
}
