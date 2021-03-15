using FluentAssertions;
using SharedKernel.ValueObjects;
using System;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.ValueObjectTests
{
    public class StateShould
    {
        [Fact]
        public void CreateState()
        {
            // Arrange
            var name = "Michigan";
            var abbreviation = "MI";

            // Act
            var state = new State(abbreviation, name);

            // Assert
            state.Name.Should().Be(name);
            state.Abbreviation.Should().Be(abbreviation);
        }

        [Fact]
        public void NotCreateStateWithEmptyName()
        {
            var name = string.Empty;
            var abbreviation = "MI";

            var exception = Assert.Throws<ArgumentException>(
                () => { new State(abbreviation, name); });

            Action action = () => new State(abbreviation, name);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(State.StateInvalidMessage);
        }

        [Fact]
        public void NotCreateStateWithEmptyAbbreviation()
        {
            var name = "Michigan";
            var abbreviation = string.Empty;

            Action action = () => new State(abbreviation, name);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(State.StateInvalidMessage);
        }

        [Fact]
        public void NotCreateStateWithNullName()
        {
            string name = null;
            var abbreviation = "MI";

            Action action = () => new State(abbreviation, name);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(State.StateInvalidMessage);
        }

        [Fact]
        public void NotCreateStateWithNullAbbreviation()
        {
            var name = "Michigan";
            string abbreviation = null;

            Action action = () => new State(abbreviation, name);

            action.Should().Throw<ArgumentException>()
                           .WithMessage(State.StateInvalidMessage);
        }


        [Fact]
        public void ReturnExpectedToString()
        {
            var name = "Michigan";
            var abbreviation = "MI";

            var state = new State(abbreviation, name);

            state.ToString().Should().Be($"{abbreviation} - {name}");
        }

        [Fact]
        public void EquateTwoStateInstancesHavingSameValues()
        {
            var name = "Michigan";
            var abbreviation = "MI";

            var state1 = new State(abbreviation, name);
            var state2 = new State(abbreviation, name);

            state1.Should().BeEquivalentTo(state2);
        }

        [Fact]
        public void NotEquateTwoStateInstancesHavingDifferingValues()
        {
            var name = "Michigan";
            var abbreviation = "MI";
            var state = new State(abbreviation, name);
            var newName = "Colorado";
            var newAbbreviation = "CO";

            var stateNew = new State(newAbbreviation, newName);
            state.Should().NotBeEquivalentTo(stateNew);
        }
    }
}
