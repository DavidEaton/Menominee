using NUnit.Framework;
using SharedKernel.ValueObjects;
using System;

namespace CustomerVehicleManagement.Tests.ValueObjectTests
{
    public class StateShould
    {
        [Test]
        public void CreateNewState()
        {
            // Arrange
            string name = "Michigan";
            string abbreviation = "MI";

            // Act
            var state = new State(abbreviation, name);

            // Assert
            Assert.That(state.Name, Is.EqualTo(state.Name));
            Assert.That(state.Abbreviation, Is.EqualTo(state.Abbreviation));
        }

        [Test]
        public void NotCreateNewStateWithEmptyName()
        {
            string name = string.Empty;
            string abbreviation = "MI";

            var exception = Assert.Throws<ArgumentException>(
                () => { new State(abbreviation, name); });

            Assert.That(exception.Message, Is.EqualTo(State.StateInvalidMessage));
        }

        [Test]
        public void NotCreateNewStateWithEmptyAbbreviation()
        {
            string name = "Michigan";
            string abbreviation = string.Empty;

            var exception = Assert.Throws<ArgumentException>(
                () => { new State(abbreviation, name); });

            Assert.That(exception.Message, Is.EqualTo(State.StateInvalidMessage));
        }

        [Test]
        public void NotCreateNewStateWithNullName()
        {
            string name = null;
            string abbreviation = "MI";

            var exception = Assert.Throws<ArgumentException>(
                () => { new State(abbreviation, name); });

            Assert.That(exception.Message, Is.EqualTo(State.StateInvalidMessage));
        }

        [Test]
        public void NotCreateNewStateWithNullAbbreviation()
        {
            string name = "Michigan";
            string abbreviation = null;

            var exception = Assert.Throws<ArgumentException>(
                () => { new State(abbreviation, name); });

            Assert.That(exception.Message, Is.EqualTo(State.StateInvalidMessage));
        }


        [Test]
        public void ReturnExpectedToString()
        {
            string name = "Michigan";
            string abbreviation = "MI";

            var state = new State(abbreviation, name);

            Assert.That(state.ToString(), Is.EqualTo($"{abbreviation} - {name}"));
        }

        [Test]
        public void EquateTwoStateInstancesHavingSameValues()
        {
            string name = "Michigan";
            string abbreviation = "MI";

            var state1 = new State(abbreviation, name);

            var state2 = new State(abbreviation, name);

            Assert.That(state1.Equals(state2));
        }

        [Test]
        public void NotEquateTwoStateInstancesHavingDifferingValues()
        {
            string name = "Michigan";
            string abbreviation = "MI";

            var state = new State(abbreviation, name);

            name = "Colorado";
            abbreviation = "CO";

            var state2 = new State(abbreviation, name);

            Assert.That(state, Is.Not.EqualTo(state2));
        }


    }
}
