using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.ValidationAttributes;
using FluentAssertions;
using SharedKernel.Enums;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.AttributeTests
{
    public class ContactableCanHaveOnlyOnePrimaryPhoneAttributeShould
    {
        [Fact]
        public void Succeed_When_Phones_Contains_One_Primary()
        {
            PersonCreateDto dto = new(Helpers.CreateValidPerson().Name, Gender.Female);
            dto.SetPhones(Helpers.CreateValidPhones());
            var attribute = new ContactableCanHaveOnlyOnePrimaryPhoneAttribute();

            var result = attribute.GetValidationResult(dto, new ValidationContext(dto));

            result.Should().Be(ValidationResult.Success);
        }


        [Fact]
        public void Not_Succeed_When_Phones_Contains_More_Than_One_Primary()
        {
            PersonCreateDto dto = new(Helpers.CreateValidPerson().Name, Gender.Female);
            dto.SetPhones(Helpers.CreateValidPhones());

            // Should we really be allowed to sneak in a second primary phone? NO!
            dto.Phones[1] = Helpers.CreateValidPrimaryPhone();

            var attribute = new ContactableCanHaveOnlyOnePrimaryPhoneAttribute();


            var result = attribute.GetValidationResult(dto, new ValidationContext(dto));
            var isSuccess = result == ValidationResult.Success;

            isSuccess.Should().BeFalse();
        }
    }
}
