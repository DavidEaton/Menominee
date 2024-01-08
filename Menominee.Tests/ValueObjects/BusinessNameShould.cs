using FluentAssertions;
using Menominee.Domain.ValueObjects;
using Xunit;

namespace Menominee.Tests.ValueObjects
{
    public class BusinessNameShould
    {
        [Fact]
        public void Create_BusinessName()
        {
            var name = "jane's";

            var businessNameOrError = BusinessName.Create(name);

            businessNameOrError.IsSuccess.Should().BeTrue();
            businessNameOrError.Value.Name.Should().Be(name);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Empty_Name()
        {
            var name = string.Empty;

            var businessNameOrError = BusinessName.Create(name);

            businessNameOrError.IsFailure.Should().BeTrue();
            businessNameOrError.Error.Should().Contain(BusinessName.InvalidLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_With_Null_Name()
        {
            string name = null;

            var businessNameOrError = BusinessName.Create(name);

            businessNameOrError.IsFailure.Should().BeTrue();
            businessNameOrError.Error.Should().Contain(BusinessName.InvalidLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_Exceeds_Maximum_Length()
        {
            string name = Utilities.LoremIpsum(BusinessName.MaximumLength + 1);

            var businessNameOrError = BusinessName.Create(name);

            businessNameOrError.IsFailure.Should().BeTrue();
            businessNameOrError.Error.Should().Contain(BusinessName.InvalidLengthMessage);

        }

        [Fact]
        public void Return_New_BusinessName_On_NewBusinessName()
        {
            string name = Utilities.LoremIpsum(BusinessName.MinimumLength + 1);
            var businessName = BusinessName.Create(name).Value;

            var newBusinessName = "New Business Name";
            var result = businessName.NewBusinessName(newBusinessName);

            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be(newBusinessName);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Invalid_NewBusinessName()
        {
            string name = Utilities.LoremIpsum(BusinessName.MinimumLength + 1);
            var businessName = BusinessName.Create(name).Value;

            var newBusinessName = Utilities.LoremIpsum(BusinessName.MinimumLength - 1);
            var result = businessName.NewBusinessName(newBusinessName);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Contain(BusinessName.InvalidLengthMessage);
        }

        [Fact]
        public void Return_IsFailure_Result_On_Create_When_Under_Minimum_Length()
        {
            string name = Utilities.LoremIpsum(BusinessName.MinimumLength - 1);

            var businessNameOrError = BusinessName.Create(name);

            businessNameOrError.IsFailure.Should().BeTrue();
            businessNameOrError.Error.Should().Contain(BusinessName.InvalidLengthMessage);

        }

        [Fact]
        public void Equate_Two_Instances_Having_Same_Values()
        {
            var name1 = "jane's";
            var name2 = "jane's";

            var businessNameOrError1 = BusinessName.Create(name1);
            var businessNameOrError2 = BusinessName.Create(name2);

            businessNameOrError1.Should().BeEquivalentTo(businessNameOrError2);
        }

        [Fact]
        public void Not_Equate_Two_Instances_Having_Differing_Values()
        {
            var name1 = "jane's";
            var name2 = "june's";

            var businessNameOrError1 = BusinessName.Create(name1);
            var businessNameOrError2 = BusinessName.Create(name2);

            businessNameOrError1.Should().NotBeEquivalentTo(businessNameOrError2);
        }

    }
}