using Bogus;
using Menominee.Domain.Entities.RepairOrders;
using FluentAssertions;
using System.Collections.Generic;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class RepairOrderSerialNumberShould
    {
        [Fact]
        public void Create_RepairOrderSerialNumber()
        {
            var faker = new Faker();
            var length = faker.Random.Int(2, 10);
            var serialNumber = faker.Random.AlphaNumeric(length);

            var result = RepairOrderSerialNumber.Create(serialNumber);

            result.Should().NotBeNull();
            result.Value.Should().BeOfType<RepairOrderSerialNumber>();
            result.Value.SerialNumber.Should().Be(serialNumber);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_With_Invalid_SerialNumber(int length)
        {
            var faker = new Faker();
            var invalidSerialNumber = faker.Random.AlphaNumeric(length);

            var result = RepairOrderSerialNumber.Create(invalidSerialNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeOneOf(RepairOrderSerialNumber.InvalidLengthMessage, RepairOrderSerialNumber.RequiredMessage);
        }

        [Fact]
        public void SetSerialNumber()
        {
            var faker = new Faker();
            var serialNumber = new RepairOrderSerialNumberFaker(true).Generate();
            var length = faker.Random.Int(2, 10);
            var newSerialNumber = faker.Random.AlphaNumeric(length);

            var result = serialNumber.SetSerialNumber(newSerialNumber);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeOfType<string>();
            result.Value.Should().Be(newSerialNumber.ToString());
            serialNumber.SerialNumber.Should().Be(newSerialNumber);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Return_Failure_On_SetSerialNumber_With_Invalid_SerialNumber(int length)
        {
            var faker = new Faker();
            var serialNumber = new RepairOrderSerialNumberFaker(true).Generate();
            var invalidSerialNumber = faker.Random.AlphaNumeric(length);

            var result = serialNumber.SetSerialNumber(invalidSerialNumber);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeOneOf(RepairOrderSerialNumber.InvalidLengthMessage, RepairOrderSerialNumber.RequiredMessage);
        }

    }
    public class TestData
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { RepairOrderSerialNumber.MinimumLength - 1 };
                yield return new object[] { RepairOrderSerialNumber.MaximumLength + 1 };
            }
        }
    }

}
