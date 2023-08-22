using Menominee.Domain.Entities.Inventory;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace Menominee.Tests.Entities
{
    public class ManufacturerShould
    {
        [Fact]
        public void Create_Manufacturer()
        {
            // Arrange
            var id = 100;
            var name = "Manufacturer One";
            var prefix = "M1";

            // Act
            var manufacturerOrError = Manufacturer.Create(id, name, prefix, new List<string>(), new List<long>());

            // Assert
            manufacturerOrError.IsFailure.Should().BeFalse();
            manufacturerOrError.Value.Should().BeOfType<Manufacturer>();
            manufacturerOrError.Value.Id.Should().Be(id);
            manufacturerOrError.Value.Name.Should().Be(name);
            manufacturerOrError.Value.Prefix.Should().Be(prefix);
        }

        [Theory]
        [MemberData(nameof(TestData.Data), MemberType = typeof(TestData))]
        public void Not_Create_Manufacturer_With_Invalid_Name(int length)
        {
            var id = 100;
            var invalidName = Utilities.RandomCharacters(length);
            var prefix = "M1";

            var manufacturerOrError = Manufacturer.Create(id, invalidName, prefix, new List<string>(), new List<long>());

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Null_Name()
        {
            var id = 100;
            string invalidName = null;
            var prefix = "M1";

            var manufacturerOrError = Manufacturer.Create(id, invalidName, prefix, new List<string>(), new List<long>());

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Invalid_Prefix()
        {
            var id = 100;
            var name = "Manufacturer One";
            var invalidPrefix = "12345";

            var manufacturerOrError = Manufacturer.Create(id, name, invalidPrefix, new List<string>(), new List<long>());

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Duplicate_Prefix()
        {
            var id = 100;
            var name = "Manufacturer One";
            var prefix = "PRE";
            var existingPrefixs = new List<string>{ "PRE" };

            var manufacturerOrError = Manufacturer.Create(id, name, prefix, existingPrefixs, new List<long>());

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Not_Create_Manufacturer_With_Duplicate_Id()
        {
            var id = 100;
            var name = "Manufacturer One";
            var prefix = "PRE";
            var existingIds = new List<long> { 100 };

            var manufacturerOrError = Manufacturer.Create(id, name, prefix, new List<string>(), existingIds);

            manufacturerOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Create_Manufacturer_With_Null_Prefix()
        {
            var id = 100;
            var name = "Manufacturer One";
            string invalidPrefix = null;
            var manufacturerOrError = Manufacturer.Create(id, name, invalidPrefix, new List<string>(), new List<long>());

            manufacturerOrError.IsFailure.Should().BeFalse();
        }


        [Fact]
        public void SetName()
        {
            var id = 100;
            var name = "Manufacturer One";
            var prefix = "M1";
            var manufacturer = Manufacturer.Create(id, name, prefix, new List<string>(), new List<long>()).Value;

            manufacturer.Name.Should().Be(name);
            var newName = "V2";
            manufacturer.SetName(newName);

            manufacturer.Name.Should().Be(newName);
        }

        [Fact]
        public void SetPrefix()
        {
            var id = 100;
            var name = "Manufacturer One";
            var prefix = "M1";
            var manufacturer = Manufacturer.Create(id, name, prefix, new List<string>(), new List<long>()).Value;

            manufacturer.Prefix.Should().Be(prefix);
            var newPrefix = "M2";
            manufacturer.SetPrefix(newPrefix, new List<string>());

            manufacturer.Prefix.Should().Be(newPrefix);
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

        [Fact]
        public void Not_Set_Prefix_With_Invalid_Prefix()
        {
            var manufacturer = CreateManufacturer();

            var newPrefix = "12345";

            var resultOrError = manufacturer.SetPrefix(newPrefix, new List<string>());
            resultOrError.IsFailure.Should().BeTrue();
        }

        [Fact]
        public void Set_Prefix_With_Null_Prefix()
        {
            var manufacturer = CreateManufacturer();

            string newPrefix = null;

            var resultOrError = manufacturer.SetPrefix(newPrefix, new List<string>());
            resultOrError.IsFailure.Should().BeFalse();
        }

        private Manufacturer CreateManufacturer()
        {
            var id = 100;
            var name = "Manufacturer One";
            var prefix = "M1";

            return Manufacturer.Create(id, name, prefix, new List<string>(), new List<long>()).Value;
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