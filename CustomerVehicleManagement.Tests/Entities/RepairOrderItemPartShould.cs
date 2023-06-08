using Bogus;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using Xunit;
namespace CustomerVehicleManagement.Tests.Entities
{
    public class RepairOrderItemPartShould
    {
        private readonly Faker faker;

        public RepairOrderItemPartShould()
        {
            faker = new Faker();
        }

        [Fact]
        public void Create_With_Valid_Input()
        {
            var list = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var cost = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var core = (double)Math.Round(faker.Random.Decimal(1, 150), 2);
            var retail = (double)Math.Round(faker.Random.Decimal(1, 1000), 2);
            var techAmount = TechAmount.Create(
                faker.PickRandom<ItemLaborType>(),
                (double)Math.Round(faker.Random.Decimal(1, 1000), 2), SkillLevel.A)
                .Value;

            var result = RepairOrderItemPart.Create(list, cost, core, retail, techAmount, false);

            result.IsSuccess.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public void Return_Failure_On_Create_With_Invalid_Input(double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string expectedErrorMessage)
        {
            var result = RepairOrderItemPart.Create(list, cost, core, retail, techAmount, fractional);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(expectedErrorMessage);
        }

        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[] { -1, 10, 10, 10, TechAmount.Create(
                    ItemLaborType.Flat, 10, SkillLevel.A).Value, false, InstallablePart.InvalidMoneyAmountMessage },
                new object[] { 10, -1, 10, 10, TechAmount.Create(
                    ItemLaborType.Flat, 10, SkillLevel.A).Value, false,  InstallablePart.InvalidMoneyAmountMessage },
                new object[] { 10, 10, -1, 10, TechAmount.Create(
                    ItemLaborType.Flat, 10, SkillLevel.A).Value, false, InstallablePart.InvalidMoneyAmountMessage },
                new object[] { 10, 10, 10, -1, TechAmount.Create(
                    ItemLaborType.Flat, 10, SkillLevel.A).Value, false, InstallablePart.InvalidMoneyAmountMessage },
                new object[] { 100000, 10, 10, 10, TechAmount.Create(
                    ItemLaborType.Flat, 10, SkillLevel.A).Value, false, InstallablePart.InvalidMoneyAmountMessage },
                new object[] { 10, 100000, 10, 10, TechAmount.Create(
                    ItemLaborType.Flat, 10, SkillLevel.A).Value, false, InstallablePart.InvalidMoneyAmountMessage },
                new object[] { 10, 10, 100000, 10, TechAmount.Create(
                    ItemLaborType.Flat, 10, SkillLevel.A).Value, false, InstallablePart.InvalidMoneyAmountMessage },
                new object[] { 10, 10, 10, 100000, TechAmount.Create(
                    ItemLaborType.Flat, 10, SkillLevel.A).Value, false, InstallablePart.InvalidMoneyAmountMessage },
            };
    }
}
