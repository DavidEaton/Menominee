using FluentAssertions;
using Menominee.Api.Features.Taxes;
using Menominee.Common.Enums;
using Menominee.Domain.Entities.Inventory;
using Menominee.Shared.Models.Taxes;
using Menominee.Tests.Helpers.Fakers;
using System.Collections.Generic;
using System.Linq;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class InventoryItemPartShould
    {
        [Fact]
        public void Create_InventoryItemPart()
        {
            // Arrange
            var fractional = false;

            // Act
            var result = InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional);

            // Assert
            result.Value.Should().BeOfType<InventoryItemPart>();
            result.IsFailure.Should().BeFalse();
        }

        [Fact]
        public void Create_InventoryItemPart_With_Optional_Line_Codes()
        {
            var fractional = false;
            string lineCode = Utilities.RandomCharacters(InventoryItemPart.MaximumLength);
            string subLineCode = Utilities.RandomCharacters(InventoryItemPart.MaximumLength);

            var result = InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional,
                lineCode, subLineCode);

            result.Value.Should().BeOfType<InventoryItemPart>();
            result.IsFailure.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_InventoryItem_With_Invalid_Money_Values(double invalidValue)
        {
            var result = InventoryItemPart.Create(
                invalidValue, invalidValue, invalidValue, invalidValue,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,
                fractional: false);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("must");
        }

        [Fact]
        public void Not_Create_InventoryItem_With_Invalid_Line_Codes()
        {
            var result = InventoryItemPart.Create(
                InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount, InstallablePart.MaximumMoneyAmount,
                TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value,

                lineCode: Utilities.RandomCharacters(InventoryItemPart.MaximumLength + 1),
                subLineCode: Utilities.RandomCharacters(InventoryItemPart.MaximumLength + 1),

                fractional: false);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("must");
        }

        [Fact]
        public void SetList()
        {
            var part = new InventoryItemPartFaker(true).Generate();
            var value = InstallablePart.MinimumMoneyAmount + 1.01;

            var result = part.SetList(value);

            result.IsFailure.Should().BeFalse();
            part.List.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_List(double invalidValue)
        {
            var part = new InventoryItemPartFaker(true).Generate();

            var result = part.SetList(invalidValue);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("must");
        }

        [Fact]
        public void SetCost()
        {
            var part = new InventoryItemPartFaker(true).Generate();
            var value = InstallablePart.MinimumMoneyAmount + 1.01;

            var result = part.SetCost(value);

            result.IsFailure.Should().BeFalse();
            part.Cost.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Cost(double invalidValue)
        {
            var part = new InventoryItemPartFaker(true).Generate();

            var result = part.SetCost(invalidValue);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("must");
        }

        [Fact]
        public void SetCore()
        {
            var part = new InventoryItemPartFaker(true).Generate();
            var value = InstallablePart.MinimumMoneyAmount + 1.01;

            var result = part.SetCore(value);

            result.IsFailure.Should().BeFalse();
            part.Core.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Core(double invalidValue)
        {
            var part = new InventoryItemPartFaker(true).Generate();

            var result = part.SetCore(invalidValue);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("must");
        }

        [Fact]
        public void SetRetail()
        {
            var part = new InventoryItemPartFaker(true).Generate();
            var value = InstallablePart.MinimumMoneyAmount + 1.01;

            var result = part.SetRetail(value);

            result.IsFailure.Should().BeFalse();
            part.Retail.Should().Be(value);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Invalid_Retail(double invalidValue)
        {
            var part = new InventoryItemPartFaker(true).Generate();

            var result = part.SetRetail(invalidValue);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("must");
        }

        [Fact]
        public void SetTechAmount()
        {
            var part = new InventoryItemPartFaker(true).Generate();
            var value = TechAmount.Create(ItemLaborType.Flat, LaborAmount.MinimumAmount, SkillLevel.A).Value;

            var result = part.SetTechAmount(value);

            result.IsFailure.Should().BeFalse();
            part.TechAmount.Should().Be(value);
        }

        [Fact]
        public void Not_Set_Null_TechAmount()
        {
            var part = new InventoryItemPartFaker(true).Generate();

            var result = part.SetTechAmount(null);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("required");
        }

        [Fact]
        public void SetLineCode()
        {
            var part = new InventoryItemPartFaker(true).Generate();
            var value = Utilities.RandomCharacters(InventoryItemPart.MaximumLength);

            var result = part.SetLineCode(value);

            result.IsFailure.Should().BeFalse();
            part.LineCode.Should().Be(value);
        }

        [Fact]
        public void Not_Set_Invalid_LineCode()
        {
            var part = new InventoryItemPartFaker(true).Generate();

            var result = part.SetLineCode(Utilities.RandomCharacters(InventoryItemPart.MaximumLength + 1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("must");
        }

        [Fact]
        public void SetSubLineCode()
        {
            var part = new InventoryItemPartFaker(true).Generate();
            var value = Utilities.RandomCharacters(InventoryItemPart.MaximumLength);

            var result = part.SetSubLineCode(value);

            result.IsFailure.Should().BeFalse();
            part.SubLineCode.Should().Be(value);
        }

        [Fact]
        public void Not_Set_Invalid_SubLineCode()
        {
            var part = new InventoryItemPartFaker(true).Generate();

            var result = part.SetSubLineCode(Utilities.RandomCharacters(InventoryItemPart.MaximumLength + 1));

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain("must");
        }

        [Theory]
        [MemberData(nameof(TestData.DataBoolean), MemberType = typeof(TestData))]
        public void SetFractional(bool value)
        {
            var part = new InventoryItemPartFaker(true).Generate();

            var result = part.SetFractional(value);

            result.IsFailure.Should().BeFalse();
            part.Fractional.Should().Be(value);
        }

        [Fact]
        public void Update_Edited_ExciseFees_On_UpdateExciseFees()
        {
            var feeCount = 3;
            var part = new InventoryItemPartFaker(true, feeCount).Generate();
            var originalFees = part.ExciseFees.Select(fee =>
            {
                return new ExciseFeeToWrite
                {
                    Id = fee.Id,
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount
                };
            }).ToList();
            var editedFees = part.ExciseFees.Select(fee =>
            {
                return new ExciseFeeToWrite
                {
                    Id = fee.Id,
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount + 1
                };
            }).ToList();
            part.ExciseFees.Should().NotBeEquivalentTo(editedFees);
            var updatedExciseFees = ExciseFeesFactory.Create(editedFees);

            var result = part.UpdateExciseFees(updatedExciseFees);

            result.IsSuccess.Should().BeTrue();
            part.ExciseFees.Count.Should().Be(feeCount);
            part.ExciseFees.Should().BeEquivalentTo(editedFees);
            part.ExciseFees.Should().NotBeEquivalentTo(originalFees);
        }

        [Fact]
        public void Add_New_Fees_On_UpdateExciseFees()
        {
            var feeCount = 3;
            var part = new InventoryItemPartFaker(true, feeCount).Generate();
            var exciseFeesToAdd = new ExciseFeeFaker(generateId: false).Generate(feeCount);
            exciseFeesToAdd.AddRange(part.ExciseFees);

            var result = part.UpdateExciseFees(exciseFeesToAdd);

            result.IsSuccess.Should().BeTrue();
            part.ExciseFees.Count.Should().Be(feeCount + feeCount);
        }

        [Fact]
        public void Remove_Deleted_Fees_On_UpdateExciseFees()
        {
            var feeCount = 3;
            var part = new InventoryItemPartFaker(true, feeCount).Generate();
            var originalFees = part.ExciseFees.Select(fee =>
            {
                return new ExciseFeeToWrite
                {
                    Id = fee.Id,
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount
                };
            }).ToList();
            var feesAfterDelete = part.ExciseFees.Select(fee =>
            {
                return new ExciseFeeToWrite
                {
                    Id = fee.Id,
                    Description = fee.Description,
                    FeeType = fee.FeeType,
                    Amount = fee.Amount + 1
                };
            }).ToList();
            feesAfterDelete.RemoveAt(0);
            part.ExciseFees.Should().NotBeEquivalentTo(feesAfterDelete);
            var updatedExciseFees = ExciseFeesFactory.Create(feesAfterDelete);

            var result = part.UpdateExciseFees(updatedExciseFees);

            result.IsSuccess.Should().BeTrue();
            part.ExciseFees.Count.Should().Be(feeCount - 1);
            part.ExciseFees.Should().BeEquivalentTo(updatedExciseFees);
            part.ExciseFees.Should().NotBeEquivalentTo(originalFees);
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { InstallablePart.MinimumMoneyAmount - .01 };
                    yield return new object[] { InstallablePart.MaximumMoneyAmount + .01 };
                }
            }
            public static IEnumerable<object[]> DataBoolean
            {
                get
                {
                    yield return new object[] { true };
                    yield return new object[] { false };
                }
            }
        }
    }
}
