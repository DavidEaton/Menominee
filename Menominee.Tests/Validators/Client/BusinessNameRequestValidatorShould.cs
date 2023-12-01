using FluentAssertions;
using Menominee.Client.Features.Contactables.Businesses;
using Menominee.Shared.Models.Businesses;
using Xunit;

namespace Menominee.Tests.Validators.Client
{
    public class BusinessNameRequestValidatorShould
    {
        private readonly BusinessNameRequestValidator validator;

        public BusinessNameRequestValidatorShould()
        {
            validator = new BusinessNameRequestValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("B")]
        [InlineData("Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis,.")]
        public void Not_Validate_With_Invalid_Input(string name)
        {
            var request = new BusinessNameRequest { Name = name };

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_With_Valid_Name()
        {
            var request = new BusinessNameRequest { Name = "Business Name" };

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }
    }

}
