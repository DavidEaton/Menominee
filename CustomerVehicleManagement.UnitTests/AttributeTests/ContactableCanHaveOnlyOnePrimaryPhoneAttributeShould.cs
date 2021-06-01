using CustomerVehicleManagement.Api.ValidationAttributes;
using CustomerVehicleManagement.Domain.Entities;
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
            Person person = new(Helpers.CreateValidPerson().Name, Gender.Female);
            person.SetPhones(Helpers.CreateValidPhones());
            var attribute = new ContactableCanHaveOnlyOnePrimaryPhoneAttribute();

            var result = attribute.GetValidationResult(person, new ValidationContext(person));

            result.Should().Be(ValidationResult.Success);
        }


        [Fact]
        public void Not_Succeed_When_Phones_Contains_More_Than_One_Primary()
        {
            Person person = new(Helpers.CreateValidPerson().Name, Gender.Female);
            person.SetPhones(Helpers.CreateValidPhones());

            // Should we really be allowed to sneak in a second primary phone? NO!
            person.Phones.Add(Helpers.CreateValidPrimaryPhone());

            var attribute = new ContactableCanHaveOnlyOnePrimaryPhoneAttribute();

            var result = attribute.GetValidationResult(person, new ValidationContext(person));
            var isSuccess = result == ValidationResult.Success;

            isSuccess.Should().BeFalse();
        }
    }
}
