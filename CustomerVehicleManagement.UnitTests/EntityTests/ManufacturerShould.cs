using CustomerVehicleManagement.Domain.Entities.Inventory;
using CustomerVehicleManagement.Shared.TestUtilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.EntityTests
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
            manufacturerOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Null_Name()
        {
            string invalidName = null;
            var prefix = "M1";
            var code = "V1";

            var manufacturerOrError = Manufacturer.Create(invalidName, prefix, code);

            manufacturerOrError.IsFailure.Should().BeTrue();
            manufacturerOrError.Error.Should().NotBeNull();
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
            manufacturerOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Null_Prefix()
        {
            var name = "Manufacturer One";
            string invalidPrefix = null;
            var code = "V1";
            var manufacturerOrError = Manufacturer.Create(name, invalidPrefix, code);

            manufacturerOrError.IsFailure.Should().BeTrue();
            manufacturerOrError.Error.Should().NotBeNull();
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
            manufacturerOrError.Error.Should().NotBeNull();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Null_Code()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            string invalidCode = null;

            var manufacturerOrError = Manufacturer.Create(name, prefix, invalidCode);

            manufacturerOrError.IsFailure.Should().BeTrue();
            manufacturerOrError.Error.Should().NotBeNull();
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
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";

            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            var newName = Utilities.RandomCharacters(length);

            Assert.Throws<ArgumentOutOfRangeException>(() => manufacturer.SetName(newName));
        }

        [Fact]
        public void Not_Set_Name_With_Null_Name()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";

            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            string newName = null;

            Assert.Throws<ArgumentOutOfRangeException>(() => manufacturer.SetName(newName));
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Prefix_With_Invalid_Prefix(int length)
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";

            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            var newPrefix = Utilities.RandomCharacters(length);

            Assert.Throws<ArgumentOutOfRangeException>(() => manufacturer.SetPrefix(newPrefix));
        }

        [Fact]
        public void Not_Set_Prefix_With_Null_Prefix()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";

            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            string newPrefix = null;

            Assert.Throws<ArgumentOutOfRangeException>(() => manufacturer.SetPrefix(newPrefix));
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Set_Code_With_Invalid_Code(int length)
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";

            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            var newCode = Utilities.RandomCharacters(length);

            Assert.Throws<ArgumentOutOfRangeException>(() => manufacturer.SetCode(newCode));
        }

        [Fact]
        public void Not_Set_Code_With_Null_Code()
        {
            var name = "Manufacturer One";
            var prefix = "M1";
            var code = "V1";

            var manufacturer = Manufacturer.Create(name, prefix, code).Value;

            string newCode = null;

            Assert.Throws<ArgumentOutOfRangeException>(() => manufacturer.SetCode(newCode));
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