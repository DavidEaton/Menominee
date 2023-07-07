using Bogus;
using CustomerVehicleManagement.Domain.Entities.RepairOrders;
using FluentAssertions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using TestingHelperLibrary.Fakers;
using Xunit;

namespace CustomerVehicleManagement.Tests.Entities
{
    public class RepairOrderWarrantyShoud
    {
        private readonly Faker faker;

        public RepairOrderWarrantyShoud()
        {
            faker = new Faker();
        }

        [Fact]
        public void Create_RepairOrderWarranty()
        {
            var length = faker.Random.Int(2, 10);
            var quantity = faker.Random.Int(2, 10);
            var type = faker.PickRandom<WarrantyType>();
            var newWarranty = faker.Random.AlphaNumeric(length);
            var originalWarranty = faker.Random.AlphaNumeric(length);
            var originalInvoiceId = faker.Random.Long(1000, 99999);

            var result = RepairOrderWarranty.Create(quantity, type, newWarranty, originalWarranty, originalInvoiceId);

            result.Should().NotBeNull();
            result.Value.Should().BeOfType<RepairOrderWarranty>();
            result.Value.Quantity.Should().Be(quantity);
            result.Value.Type.Should().Be(type);
            result.Value.NewWarranty.Should().Be(newWarranty);
            result.Value.OriginalWarranty.Should().Be(originalWarranty);
            result.Value.OriginalInvoiceId.Should().Be(originalInvoiceId);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidQuantity), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_RepairOrderWarranty_With_Invalid_Quantity(int invalidQuantity)
        {
            var length = faker.Random.Int(2, 10);
            var type = faker.PickRandom<WarrantyType>();
            var newWarranty = faker.Random.AlphaNumeric(length);
            var originalWarranty = faker.Random.AlphaNumeric(length);
            var originalInvoiceId = faker.Random.Long(1000, 99999);

            var result = RepairOrderWarranty.Create(invalidQuantity, type, newWarranty, originalWarranty, originalInvoiceId);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderWarranty.InvalidValueMessage);
        }

        [Fact]
        public void Return_Failure_On_Create_RepairOrderWarranty_With_Invalid_WarrantyType()
        {
            var length = faker.Random.Int(2, 10);
            var quantity = faker.Random.Int(2, 10);
            WarrantyType invalidWarrantyType = (WarrantyType)(-1);
            var newWarranty = faker.Random.AlphaNumeric(length);
            var originalWarranty = faker.Random.AlphaNumeric(length);
            var originalInvoiceId = faker.Random.Long(1000, 99999);

            var result = RepairOrderWarranty.Create(quantity, invalidWarrantyType, newWarranty, originalWarranty, originalInvoiceId);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderWarranty.RequiredMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidLength), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_RepairOrderWarranty_With_Invalid_NewWarranty(int invalidLength)
        {
            var length = faker.Random.Int(2, 10);
            var type = faker.PickRandom<WarrantyType>();
            var invalidNewWarranty = faker.Random.AlphaNumeric(invalidLength);
            var originalWarranty = faker.Random.AlphaNumeric(length);
            var originalInvoiceId = faker.Random.Long(1000, 99999);

            var result = RepairOrderWarranty.Create(length, type, invalidNewWarranty, originalWarranty, originalInvoiceId);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderWarranty.InvalidLengthMessage);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidLength), MemberType = typeof(TestData))]
        public void Return_Failure_On_Create_RepairOrderWarranty_With_Invalid_OriginalWarranty(int invalidLength)
        {
            var length = faker.Random.Int(2, 10);
            var type = faker.PickRandom<WarrantyType>();
            var newWarranty = faker.Random.AlphaNumeric(length);
            var invalidOriginalWarranty = faker.Random.AlphaNumeric(invalidLength);
            var originalInvoiceId = faker.Random.Long(1000, 99999);

            var result = RepairOrderWarranty.Create(length, type, newWarranty, invalidOriginalWarranty, originalInvoiceId);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderWarranty.InvalidLengthMessage);
        }

        [Fact]
        public void SetQuantity()
        {
            var warranty = new RepairOrderWarrantyFaker(true).Generate();
            var newQuantity = faker.Random.Int(2, 10);
            warranty.Quantity.Should().NotBe(newQuantity);

            var result = warranty.SetQuantity(newQuantity);

            result.IsSuccess.Should().BeTrue();
            warranty.Quantity.Should().Be(newQuantity);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidQuantity), MemberType = typeof(TestData))]
        public void Return_Failure_On_Set_Invalid_Quantity(int invalidQuantity)
        {
            var warranty = new RepairOrderWarrantyFaker(true).Generate();

            var result = warranty.SetQuantity(invalidQuantity);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderWarranty.InvalidValueMessage);
        }

        [Fact]
        public void SetType()
        {
            var warranty = new RepairOrderWarrantyFaker(true).Generate();
            var availableTypesNotInUse = Enum
                .GetValues(typeof(WarrantyType))
                    .Cast<WarrantyType>()
                .Where(type => type != warranty.Type)
                .ToList();
            var newType = faker.PickRandom(availableTypesNotInUse);
            warranty.Type.Should().NotBe(newType);

            var result = warranty.SetType(newType);

            result.IsSuccess.Should().BeTrue();
            warranty.Type.Should().Be(newType);
        }

        [Fact]
        public void Return_Failure_On_Set_Invalid_Type()
        {
            var warranty = new RepairOrderWarrantyFaker(true).Generate();
            WarrantyType invalidWarrantyType = (WarrantyType)(-1);

            var result = warranty.SetType(invalidWarrantyType);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderWarranty.RequiredMessage);
        }

        [Fact]
        public void SetNewWarranty()
        {
            var length = faker.Random.Int(2, 10);
            var warranty = new RepairOrderWarrantyFaker(true).Generate();
            var newWarranty = faker.Random.AlphaNumeric(length);
            warranty.NewWarranty.Should().NotBe(newWarranty);

            var result = warranty.SetNewWarranty(newWarranty);

            result.IsSuccess.Should().BeTrue();
            warranty.NewWarranty.Should().Be(newWarranty);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidLength), MemberType = typeof(TestData))]
        public void Return_Failure_On_Set_Invalid_NewWarranty(int invalidLength)
        {
            var warranty = new RepairOrderWarrantyFaker(true).Generate();
            var newWarranty = faker.Random.AlphaNumeric(invalidLength);

            var result = warranty.SetNewWarranty(newWarranty);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderWarranty.InvalidLengthMessage);
        }

        [Fact]
        public void SetOriginalWarranty()
        {
            var length = faker.Random.Int(2, 10);
            var warranty = new RepairOrderWarrantyFaker(true).Generate();
            var newOriginalWarranty = faker.Random.AlphaNumeric(length);
            warranty.OriginalWarranty.Should().NotBe(newOriginalWarranty);

            var result = warranty.SetOriginalWarranty(newOriginalWarranty);

            result.IsSuccess.Should().BeTrue();
            warranty.OriginalWarranty.Should().Be(newOriginalWarranty);
        }

        [Theory]
        [MemberData(nameof(TestData.InvalidLength), MemberType = typeof(TestData))]
        public void Return_Failure_On_Set_Invalid_OriginalWarranty(int invalidLength)
        {
            var warranty = new RepairOrderWarrantyFaker(true).Generate();
            var invalidOriginalWarranty = faker.Random.AlphaNumeric(invalidLength);

            var result = warranty.SetOriginalWarranty(invalidOriginalWarranty);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(RepairOrderWarranty.InvalidLengthMessage);
        }

        public class TestData
        {
            public static IEnumerable<object[]> InvalidLength
            {
                get
                {
                    yield return new object[] { null };
                    yield return new object[] { RepairOrderWarranty.MinimumLength - 1 };
                    yield return new object[] { RepairOrderWarranty.MaximumLength + 1 };
                }
            }

            public static IEnumerable<object[]> InvalidQuantity
            {
                get
                {
                    yield return new object[] { RepairOrderWarranty.MaximumValue + 1 };
                }
            }
        }
    }
}