using CustomerVehicleManagement.Api.Data.Dtos;
using CustomerVehicleManagement.Api.Data.ValidationAttributes;
using FluentAssertions;
using SharedKernel.Enums;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.AttributeTests
{
    public class PersonCanHaveOnlyOnePrimaryPhoneAttributeShould
    {
        [Fact]
        public void Succeed_When_Phones_Contains_More_Than_One_Primary()
        {
            PersonCreateDto dto = new(Helpers.CreateValidPerson().Name, Gender.Female);
            dto.Phones = Helpers.CreatePhoneCreateDtos();
            var attribute = new PersonCanHaveOnlyOnePrimaryPhoneAttribute();
            
            var result = attribute.GetValidationResult(dto, new ValidationContext(dto));

            result.Should().Be(ValidationResult.Success);
        }


        [Fact]
        public void Not_Succeed_When_Phones_Contains_More_Than_One_Primary()
        {
            PersonCreateDto dto = new(Helpers.CreateValidPerson().Name, Gender.Female);
            dto.Phones = Helpers.CreatePhoneCreateDtos();

            foreach (var phone in dto.Phones)
            {
                phone.Primary = true;
            }

            var attribute = new PersonCanHaveOnlyOnePrimaryPhoneAttribute();


            var result = attribute.GetValidationResult(dto, new ValidationContext(dto));
            var isSuccess = result == ValidationResult.Success;

            isSuccess.Should().BeFalse();
        }
    }
}
