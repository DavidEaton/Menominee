using FluentAssertions;
using SharedKernel.Static;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.OtherTests
{
    public class StatesCollectionShould
    {

        [Fact]
        public void ReturnStatesList()
        {
            var statesList = States.ToList();

            statesList.Should().NotBeNull();
            statesList.Count.Should().BeGreaterThan(50);
        }

        [Fact]
        public void ReturnAbbreviationsList()
        {
            var stateAbbreviationsList = States.Abbreviations();

            stateAbbreviationsList.Should().Contain("MI");
        }

        [Fact]
        public void ReturnStatesNamesList()
        {
            var statesNamesList = States.Names();

            statesNamesList.Should().Contain("Michigan");
        }

        [Fact]
        public void GetStateNameOnGetName()
        {
            var stateName = States.GetName("MI");

            stateName.Should().Be(stateName);
        }

        [Fact]
        public void GetStateAbbreviationOnGetAbbreviation()
        {
            var stateAbbreviation = States.GetAbbreviation("Michigan");

            stateAbbreviation.Should().Be("MI");
        }

    }

}
