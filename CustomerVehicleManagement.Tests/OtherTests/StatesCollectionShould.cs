using NUnit.Framework;
using SharedKernel.Static;

namespace CustomerVehicleManagement.Tests.OtherTests
{
    public class StatesCollectionShould
    {

        [Test]
        public void ReturnStatesList()
        {
            var statesList = States.ToList();

            Assert.That(statesList, Is.Not.Null);
            Assert.That(statesList.Count > 10);
        }

        [Test]
        public void ReturnAbbreviationsList()
        {
            var stateAbbreviationsList = States.Abbreviations();

            Assert.That(stateAbbreviationsList.Contains("MI"));
        }

        [Test]
        public void ReturnStatesNamesList()
        {
            var statesNamesList = States.Names();

            Assert.That(statesNamesList.Contains("Michigan"));
        }

        [Test]
        public void GetStateNameOnGetName()
        {
            var stateName = States.GetName("MI");

            Assert.That(stateName.Equals("Michigan"));
        }

        [Test]
        public void GetStateAbbreviationOnGetAbbreviation()
        {
            var stateAbbreviation = States.GetAbbreviation("Michigan");

            Assert.That(stateAbbreviation.Equals("MI"));
        }

    }
}
