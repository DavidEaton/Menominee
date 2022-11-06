using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.Tests.Unit.Entities
{
    public class ManufacturerShould
    {
        [Fact]
        public void Create_Manufacturer()
        {
            // Arrange
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";

            // Act
            var manufacturerOrError = Manufacturer.Create(name, prefix, code);

            // Assert
            manufacturerOrError.IsFailure.Should().BeFalse();
            manufacturerOrError.Value.Should().BeOfType<Manufacturer>();
            manufacturerOrError.Value.Name.Should().Be(name);
            manufacturerOrError.Value.Prefix.Should().Be(prefix);
            manufacturerOrError.Value.Code.Should().Be(code);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_Manufacturer_With_Invalid_Name(int length)
        {
            var invalidName = Utilities.RandomCharacters(length);
            var prefix = "M1";
            var code = "V1";

            var manufacturerOrError = Manufacturer.Create(invalidName, prefix, code);

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Null_Name()
        {
            string invalidName = null;
            var prefix = "M1";
            var code = "V1";

            var manufacturerOrError = Manufacturer.Create(invalidName, prefix, code);

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_Manufacturer_With_Invalid_Prefix(int length)
        {
            var name = "Manufacturer One";
            var invalidPrefix = Utilities.RandomCharacters(length);
            var code = "V1";

            var manufacturerOrError = Manufacturer.Create(name, invalidPrefix, code);

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Null_Prefix()
        {
            var name = "Manufacturer One";
            string invalidPrefix = null;
            var code = "V1";
            var manufacturerOrError = Manufacturer.Create(name, invalidPrefix, code);

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_Manufacturer_With_Invalid_Code(int length)
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var invalidCode = Utilities.RandomCharacters(length);

            var manufacturerOrError = Manufacturer.Create(name, prefix, invalidCode);

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Null_Code()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            string invalidCode = null;

            var manufacturerOrError = Manufacturer.Create(name, prefix, invalidCode);

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void SetName()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";
            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            manufacturer.Name.Should().Be(name);
            var newName = "V2";
            manufacturer.SetName(newName);

            manufacturer.Name.Should().Be(newName);
        }

        [Fact]
        public void SetPrefix()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";
            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            manufacturer.Prefix.Should().Be(prefix);
            var newPrefix = "M2";
            manufacturer.SetPrefix(newPrefix);

            manufacturer.Prefix.Should().Be(newPrefix);
        }

        [Fact]
        public void SetCode()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";
            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            manufacturer.Code.Should().Be(code);
            var newCode = "V2";
            manufacturer.SetCode(newCode);

            manufacturer.Code.Should().Be(newCode);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Name_With_Invalid_Name(int length)
        {
            var manufacturer = CreateManufacturer();
            var newName = Utilities.RandomCharacters(length);

            var resultOrError = manufacturer.SetName(newName);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Name_With_Null_Name()
        {
            var manufacturer = CreateManufacturer();

            var resultOrError = manufacturer.SetName(null);

            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Prefix_With_Invalid_Prefix(int length)
        {
            var manufacturer = CreateManufacturer();

            var newPrefix = Utilities.RandomCharacters(length);

            var resultOrError = manufacturer.SetPrefix(newPrefix);
            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Prefix_With_Null_Prefix()
        {
            var manufacturer = CreateManufacturer();

            string newPrefix = null;

            var resultOrError = manufacturer.SetPrefix(newPrefix);
            resultOrError.IsFailure.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Code_With_Invalid_Code(int length)
        {
            var manufacturer = CreateManufacturer();

            var newCode = Utilities.RandomCharacters(length);

            var resultOrError = manufacturer.SetCode(newCode);
            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Set_Code_With_Null_Code()
        {
            var manufacturer = CreateManufacturer();

            string newCode = null;

            var resultOrError = manufacturer.SetCode(newCode);
            resultOrError.IsFailure.Should().BeTrue();
        }

        private Manufacturer CreateManufacturer()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";

            return Manufacturer.Create(name, prefix, code).Value;
        }

        internal class TestData
        {
            public static IEnumerable<object[]> Data
            {
                get
                {
                    yield return new object[] { Manufacturer.MinimumLength - 1 };
                    yield return new object[] { Manufacturer.MaximumLength + 1 };
                }
            }
        }
    }
}