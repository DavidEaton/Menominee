using FluentAssertions;
using FluentAssertions.Execution;
using SharedKernel.ValueObjects;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitFacts.ValueObjectFacts
{
    public class PersonNameShould
    {
        [Fact]
        public void CreatePersonName()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";

            // Act
            var name = new PersonName(lastName, firstName);

            // Assert
            name.Should().NotBeNull();
        }

        [Fact]
        public void NotCreatePersonNameWithEmptyName()
        {
            var firstName = "";
            var lastName = "";

            var exception = Assert.Throws<ArgumentException>(
                () => { new PersonName(firstName, lastName); });

            Action action = () => new PersonName(firstName, lastName);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(PersonName.PersonNameEmptyMessage);
        }

        [Fact]
        public void NotCreatePersonNameWithNullName()
        {
            string firstName = null;
            string lastName = null;

            Action action = () => new PersonName(firstName, lastName);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(PersonName.PersonNameEmptyMessage);
        }

        [Fact]
        public void CreatePersonNameWithEmptyLastName()
        {
            var firstName = "Molly";
            var lastName = "";

            var name = new PersonName(lastName, firstName);

            name.Should().NotBeNull();
        }

        [Fact]
        public void CreatePersonNameWithEmptyFirstName()
        {
            var firstName = "";
            var lastName = "Moops";

            var name = new PersonName(lastName, firstName);

            name.Should().NotBeNull();
        }

        [Fact]
        public void CreatePersonNameWithNullLastName()
        {
            var firstName = "Molly";
            string lastName = null;

            var name = new PersonName(lastName, firstName);

            name.Should().NotBeNull();
        }

        [Fact]
        public void CreatePersonWithNameNullFirstName()
        {
            string firstName = null;
            var lastName = "Moops";

            var name = new PersonName(lastName, firstName);

            name.Should().NotBeNull();
        }

        [Fact]
        public void EquateTwoPersonNameInstancesHavingSameValues()
        {
            var firstName = "Jane";
            var lastName = "Doe";

            var name1 = new PersonName(lastName, firstName);
            var name2 = new PersonName(lastName, firstName);

            name1.Should().BeEquivalentTo(name2);
        }

        [Fact]
        public void NotEquateTwoPersonNameInstancesHavingDifferingValues()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var newFirstName = "Jim";
            var newLastName = "Doane";

            var nameNew = new PersonName(newFirstName, newLastName);

            name.Should().NotBeEquivalentTo(nameNew);
        }

        [Fact]
        public void ReturnNewPersonNameOnNewLastName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var newLastName = "Smith";

            name = name.NewLastName(newLastName);

            name.LastName.Should().Be(newLastName);
        }

        [Fact]
        public void ReturnNewPersonNameOnNewFirstName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var name = new PersonName(lastName, firstName);
            var newFirstName = "Smith";

            name = name.NewFirstName(newFirstName);

            name.FirstName.Should().Be(newFirstName);
        }

        [Fact]
        public void ReturnNewPersonNameOnNewMiddleName()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var middleName = "Jingleheimer";
            var name = new PersonName(lastName, firstName, middleName);
            var newMiddleName = "Allabaster";

            name = name.NewMiddleName(newMiddleName);

            name.MiddleName.Should().Be(newMiddleName);
        }


        [Fact]
        public void ReturnAllPersonNameVariants()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var middleName = "The";

            var name = new PersonName(lastName, firstName, middleName);

            using (new AssertionScope())
            {
                name.FirstMiddleLast.Should().Be($"{firstName} {middleName} {lastName}");
                name.LastFirstMiddle.Should().Be($"{lastName}, {firstName} {middleName}");
                name.LastFirstMiddleInitial.Should().Be($"{lastName}, {firstName} {middleName[0]}.");
            }
        }
    }
}
