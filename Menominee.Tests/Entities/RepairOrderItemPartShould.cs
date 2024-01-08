using Bogus;
using FluentAssertions;
using Menominee.Domain.Entities.Inventory;
using Menominee.Domain.Entities.RepairOrders;
using Menominee.Domain.Entities.Taxes;
using Menominee.Domain.Enums;
using System;
using System.Collections.Generic;
using TestingHelperLibrary.Fakers;
using Xunit;
namespace Menominee.Tests.Entities
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

        [Fact]
        public void Calculate_ExciseFeesTotal_Correctly()
        {
            var exciseFees = new List<ExciseFee>
            {
                CreateExciseFee(10),
                CreateExciseFee(20),
                CreateExciseFee(30)
            };

            var list = 100;
            var cost = 200;
            var core = 300;
            var retail = 400;
            var techAmount = TechAmount.Create(ItemLaborType.Flat, 500, SkillLevel.A).Value;
            var fractional = false;

            var repairOrderItemPartResult = RepairOrderItemPart.Create(list, cost, core, retail, techAmount, fractional, exciseFees: exciseFees);
            var repairOrderItemPart = repairOrderItemPartResult.Value;

            var total = repairOrderItemPart.ExciseFeesTotal;

            total.Should().Be(60); // 10 + 20 + 30
        }

        [Fact]
        public void Return_Failure_On_Create_With_Invalid_Length_LineCode()
        {
            var lineCode = faker.Random.String2(RepairOrderItemPart.MaximumLength + 1);
            var result = RepairOrderItemPart.Create(10, 10, 10, 10, TechAmount.Create(ItemLaborType.Flat, 10, SkillLevel.A).Value, false, lineCode);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemPart.InvalidLengthMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_With_Long_Invalid_Length_SubLineCode()
        {
            var subLineCode = faker.Random.String2(RepairOrderItemPart.MaximumLength + 1);
            var result = RepairOrderItemPart.Create(10, 10, 10, 10, TechAmount.Create(ItemLaborType.Flat, 10, SkillLevel.A).Value, false, null, subLineCode);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderItemPart.InvalidLengthMessage);
        }

        [Fact]
        public void Handle_Null_LineCode_And_SubLineCode_On_Create()
        {
            var result = RepairOrderItemPart.Create(10, 10, 10, 10, TechAmount.Create(ItemLaborType.Flat, 10, SkillLevel.A).Value, false, null, null);

            result.IsSuccess.Should().BeTrue();
            result.Value.LineCode.Should().BeEmpty();
            result.Value.SubLineCode.Should().BeEmpty();
        }

        [Fact]
        public void Calculate_ExciseFeesTotal_As_Zero_When_No_ExciseFees()
        {
            var list = 100;
            var cost = 200;
            var core = 300;
            var retail = 400;
            var techAmount = TechAmount.Create(ItemLaborType.Flat, 500, SkillLevel.A).Value;
            var fractional = false;

            var result = RepairOrderItemPart.Create(list, cost, core, retail, techAmount, fractional);

            result.Value.ExciseFeesTotal.Should().Be(0);
        }

        [Fact]
        public void Return_Failure_When_SetList_Is_Out_Of_Range()
        {
            var part = CreateValidPart();
            var result = part.SetList(InstallablePart.MaximumMoneyAmount + 1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(InstallablePart.InvalidMoneyAmountMessage);
        }

        [Fact]
        public void Return_Failure_When_SetCost_Is_Out_Of_Range()
        {
            var part = CreateValidPart();
            var result = part.SetCost(InstallablePart.MaximumMoneyAmount + 1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(InstallablePart.InvalidMoneyAmountMessage);
        }

        [Fact]
        public void Return_Failure_When_SetCore_Is_Out_Of_Range()
        {
            var part = CreateValidPart();
            var result = part.SetCore(InstallablePart.MaximumMoneyAmount + 1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(InstallablePart.InvalidMoneyAmountMessage);
        }

        [Fact]
        public void Return_Failure_When_SetRetail_Is_Out_Of_Range()
        {
            var part = CreateValidPart();
            var result = part.SetRetail(InstallablePart.MaximumMoneyAmount + 1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(InstallablePart.InvalidMoneyAmountMessage);
        }

        [Fact]
        public void Return_Failure_When_SetTechAmount_Is_Null()
        {
            var part = CreateValidPart();
            var result = part.SetTechAmount(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(InstallablePart.RequiredMessage);
        }

        [Fact]
        public void Return_Failure_When_SetLineCode_Is_Invalid_Length()
        {
            var part = CreateValidPart();
            var lineCode = faker.Random.String2(InstallablePart.MaximumLineCodeLength + 1);
            var result = part.SetLineCode(lineCode);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(InstallablePart.InvalidLineCodeLengthMessage);
        }

        [Fact]
        public void Return_Failure_When_SetSubLineCode_Is_Invalid_Length()
        {
            var part = CreateValidPart();
            var subLineCode = faker.Random.String2(InstallablePart.MaximumLineCodeLength + 1);
            var result = part.SetSubLineCode(subLineCode);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(InstallablePart.InvalidLineCodeLengthMessage);
        }

        [Fact]
        public void AddExciseFee()
        {
            var part = CreateValidPart();
            part.ExciseFees.Count.Should().Be(0);
            var fee = new ExciseFeeFaker(true).Generate();

            var result = part.AddExciseFee(fee);

            result.IsSuccess.Should().BeTrue();
            part.ExciseFees.Count.Should().Be(1);
            part.ExciseFees.Should().Contain(fee);
        }

        [Fact]
        public void Return_Failure_On_Add_Invalid_ExciseFee()
        {
            var part = CreateValidPart();
            part.ExciseFees.Count.Should().Be(0);
            ExciseFee invalidFee = null;

            var result = part.AddExciseFee(invalidFee);

            result.IsFailure.Should().BeTrue();
            part.ExciseFees.Count.Should().Be(0);
            result.Error.Should().Be(InstallablePart.RequiredMessage);
        }

        [Fact]
        public void RemoveExciseFee()
        {
            var part = CreateValidPart();
            part.ExciseFees.Count.Should().Be(0);
            var fee = new ExciseFeeFaker(true).Generate();
            part.AddExciseFee(fee);
            part.AddExciseFee(new ExciseFeeFaker(true).Generate());
            part.ExciseFees.Count.Should().Be(2);

            var result = part.RemoveExciseFee(fee);

            result.IsSuccess.Should().BeTrue();
            part.ExciseFees.Count.Should().Be(1);
            part.ExciseFees.Should().NotContain(fee);
        }

        [Fact]
        public void Return_Failure_On_Remove_Invalid_ExciseFee()
        {
            var part = CreateValidPart();
            part.ExciseFees.Count.Should().Be(0);
            var fee = new ExciseFeeFaker(true).Generate();
            part.AddExciseFee(fee);
            part.AddExciseFee(new ExciseFeeFaker(true).Generate());
            part.ExciseFees.Count.Should().Be(2);
            fee = null;

            var result = part.RemoveExciseFee(fee);

            result.IsFailure.Should().BeTrue();
            part.ExciseFees.Count.Should().Be(2);
            result.Error.Should().Be(InstallablePart.RequiredMessage);
        }

        private static RepairOrderItemPart CreateValidPart()
        {
            return RepairOrderItemPart.Create(10, 10, 10, 10, TechAmount.Create(ItemLaborType.Flat, 10, SkillLevel.A).Value, false).Value;
        }

        private ExciseFee CreateExciseFee(double amount)
        {
            var description = faker.Random.String2(1, SalesTax.DescriptionMaximumLength);
            var feeType = faker.PickRandom<ExciseFeeType>();
            return ExciseFee.Create(
                description,
                feeType,
                amount)
                .Value;
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
