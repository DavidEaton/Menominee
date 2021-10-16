using CSharpFunctionalExtensions;
using FluentAssertions;
using Menominee.Common.Utilities;
using Menominee.Common.ValueObjects;
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
            organizationNameOrError.Value.Name.Should().Be(name);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_Name()
        {
            var name = string.Empty;

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Be(OrganizationName.MinimumLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_Name()
        {
            string name = null;

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Be(OrganizationName.MinimumLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_Exceeds_Maximum_Length()
        {
            string name = Helpers.LoremIpsum(OrganizationName.MaximumLength + 1);

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Be(OrganizationName.MaximumLengthMessage);

        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_Under_Minimum_Length()
        {
            string name = Helpers.LoremIpsum(OrganizationName.MinimumLength - 1);

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Be(OrganizationName.MinimumLengthMessage);

        }

        [Fact]
        public void Equate_Two_OrganizationName_Instances_Having_Same_Values()
        {
            var name1 = "june's";
            var name2 = "june's";

            var organizationNameOrError1 = OrganizationName.Create(name1);
            var organizationNameOrError2 = OrganizationName.Create(name2);


            organizationNameOrError1.Should().BeEquivalentTo(organizationNameOrError2);
        }

        [Fact]
        public void Not_Equate_Two_OrganizationName_Instances_Having_Differing_Values()
        {
            var name1 = "jane's";
            var name2 = "june's";

            var organizationNameOrError1 = OrganizationName.Create(name1);
            var organizationNameOrError2 = OrganizationName.Create(name2);

            organizationNameOrError1.Should().NotBeEquivalentTo(organizationNameOrError2);
        }

        [Fact]
        public void Return_New_OrganizationName_On_NewOrganizationName()
        {
            var name = "jane's";
            Result<OrganizationName> organizationNameOrError = OrganizationName.Create(name);
            organizationNameOrError.Value.Name.Should().Be(name);
            name  = "June's";

            string newOrganizationName = OrganizationName.NewOrganizationName(name).Name;

            newOrganizationName.Should().Be(name);
        }

    }
}
