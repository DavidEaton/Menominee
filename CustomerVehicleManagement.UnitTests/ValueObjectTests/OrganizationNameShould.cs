using FluentAssertions;
using SharedKernel.Utilities;
using SharedKernel.ValueObjects;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.ValueObjectTests
{
    public class OrganizationNameShould
    {
        [Fact]
        public void Create_OrganizationName()
        {
            var name = "jane's";

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsSuccess.Should().BeTrue();
            organizationNameOrError.Value.Value.Should().Be(name);
        }

        [Fact]
        public void Not_Create_OrganizationName_With_Empty_Name()
        {
            var name = string.Empty;

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Be(OrganizationName.OrganizationNameEmptyMessage);
        }

        [Fact]
        public void Not_Create_OrganizationName_With_Null_Name()
        {
            string name = null;

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Be(OrganizationName.OrganizationNameEmptyMessage);
        }

        [Fact]
        public void Not_Create_OrganizationName_That_Exceeds_Maximum_Length()
        {
            string name = Helpers.LoremIpsum(OrganizationName.MaximumLength + 1);

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Be(OrganizationName.OrganizationNameInvalidMessage);

        }

        [Fact]
        public void EquateTwoOrganizationNameInstancesHavingSameValues()
        {
            var name1 = "june's";
            var name2 = "june's";

            var organizationNameOrError1 = OrganizationName.Create(name1);
            var organizationNameOrError2 = OrganizationName.Create(name2);


            organizationNameOrError1.Should().BeEquivalentTo(organizationNameOrError2);
        }

        [Fact]
        public void NotEquateTwoOrganizationNameInstancesHavingDifferingValues()
        {
            var name1 = "jane's";
            var name2 = "june's";

            var organizationNameOrError1 = OrganizationName.Create(name1);
            var organizationNameOrError2 = OrganizationName.Create(name2);

            organizationNameOrError1.Should().NotBeEquivalentTo(organizationNameOrError2);
        }

        [Fact]
        public void ReturnNewOnNewOrganizationName()
        {
            var name = "jane's";
            Result<OrganizationName> organizationNameOrError = OrganizationName.Create(name);
            organizationNameOrError.Value.Value.Should().Be(name);
            name  = "June's";

            string newOrganizationName = OrganizationName.NewOrganizationName(name).Value;

            newOrganizationName.Should().Be(name);
        }

    }
}
