using FluentAssertions;
using Menominee.Common.ValueObjects;
using Xunit;

namespace Menominee.Tests.ValueObjects
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
            organizationNameOrError.Error.Should().Contain(OrganizationName.InvalidMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_Name()
        {
            string name = null;

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Contain(OrganizationName.InvalidMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_Exceeds_Maximum_Length()
        {
            string name = Utilities.LoremIpsum(OrganizationName.MaximumLength + 1);

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Contain(OrganizationName.InvalidMessage);

        }

        [Fact]
        public void Return_New_OrganizationName_On_NewOrganizationName()
        {
            string name = Utilities.LoremIpsum(OrganizationName.MinimumLength + 1);
            var organizationName = OrganizationName.Create(name).Value;

            var newOrganizationName = "New Organization Name";
            var result = organizationName.NewOrganizationName(newOrganizationName);

            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(newOrganizationName);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Invalid_NewOrganizationName()
        {
            string name = Utilities.LoremIpsum(OrganizationName.MinimumLength + 1);
            var organizationName = OrganizationName.Create(name).Value;

            var newOrganizationName = Utilities.LoremIpsum(OrganizationName.MinimumLength - 1);
            var result = organizationName.NewOrganizationName(newOrganizationName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain(OrganizationName.InvalidMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_Under_Minimum_Length()
        {
            string name = Utilities.LoremIpsum(OrganizationName.MinimumLength - 1);

            var organizationNameOrError = OrganizationName.Create(name);

            organizationNameOrError.IsFailure.Should().BeTrue();
            organizationNameOrError.Error.Should().Contain(OrganizationName.InvalidMessage);

        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var name1 = "jane's";
            var name2 = "jane's";

            var organizationNameOrError1 = OrganizationName.Create(name1);
            var organizationNameOrError2 = OrganizationName.Create(name2);

            organizationNameOrError1.Should().BeEquivalentTo(organizationNameOrError2);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var name1 = "jane's";
            var name2 = "june's";

            var organizationNameOrError1 = OrganizationName.Create(name1);
            var organizationNameOrError2 = OrganizationName.Create(name2);

            organizationNameOrError1.Should().NotBeEquivalentTo(organizationNameOrError2);
        }

    }
}