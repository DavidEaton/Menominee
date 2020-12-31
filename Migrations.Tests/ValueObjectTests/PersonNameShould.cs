using Migrations.Core.ValueObjects;
using NUnit.Framework;
using System;

namespace Migrations.Tests.ValueObjectTests
{
    public class PersonNameShould
    {
        [Test]
        public void CreateNewPersonName()
        {
            // Arrange
            string firstName = "Jane";
            string lastName = "Doe";

            // Act
            var name = new PersonName(lastName, firstName);

            // Assert
            Assert.That(name, Is.Not.Null);
        }

        [Test]
        public void NotCreateNewPersonNameWithEmptyName()
        {
            string firstName = null;
            string lastName = null;

            var exception = Assert.Throws<ArgumentException>(
                () => { new PersonName(firstName, lastName); });

            Assert.That(exception.Message, Is.EqualTo(PersonName.PersonNameEmptyMessage));
        }

        [Test]
        public void EquateTwoPersonNameInstancesHavingSameValues()
        {
            var firstName = "Jane";
            var lastName = "Doe";

            var name1 = new PersonName(lastName, firstName);
            var name2 = new PersonName(lastName, firstName);

            Assert.That(name1.Equals(name2));
        }

        [Test]
        public void NotEquateTwoPersonNameInstancesHavingDifferingValues()
        {
            var firstName = "Jane";
            var lastName = "Doe";

            var name1 = new PersonName(lastName, firstName);

            firstName = "Jim";
            lastName = "Doane";
            var name2 = new PersonName(lastName, firstName);

            Assert.That(name1, Is.Not.EqualTo(name2));
        }

        [Test]
        public void ReturnAllPersonNameVariants()
        {
            var firstName = "Jane";
            var lastName = "Doe";
            var middleName = "The";

            var name = new PersonName(lastName, firstName, middleName);

            Assert.That($"{firstName} {middleName} {lastName}", Is.EqualTo(name.FirstMiddleLast));
            Assert.That($"{lastName}, {firstName} {middleName}", Is.EqualTo(name.LastFirstMiddle));
            Assert.That($"{lastName}, {firstName} {middleName[0]}.", Is.EqualTo(name.LastFirstMiddleInitial));
        }
    }
}
