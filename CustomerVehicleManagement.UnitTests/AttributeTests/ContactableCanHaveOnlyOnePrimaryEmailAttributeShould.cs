﻿using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.ValidationAttributes;
using FluentAssertions;
using SharedKernel.Enums;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.AttributeTests
{
    public class ContactableCanHaveOnlyOnePrimaryEmailAttributeShould
    {
        [Fact]
        public void Succeed_When_Emails_Contains_One_Primary()
        {
            PersonCreateDto person = new(Helpers.CreateValidPerson().Name, Gender.Female);
            person.SetEmails(Helpers.CreateValidEmails());
            var attribute = new ContactableCanHaveOnlyOnePrimaryEmailAttribute();

            var result = attribute.GetValidationResult(person, new ValidationContext(person));

            result.Should().Be(ValidationResult.Success);
        }


        [Fact]
        public void Not_Succeed_When_Emails_Contains_More_Than_One_Primary()
        {
            PersonCreateDto dto = new(Helpers.CreateValidPerson().Name, Gender.Female);
            dto.SetEmails(Helpers.CreateValidEmails());

            // Should we really be allowed to sneak in a second primary email? NO!
            dto.Emails[1] = Helpers.CreateValidPrimaryEmail();

            var attribute = new ContactableCanHaveOnlyOnePrimaryEmailAttribute();


            var result = attribute.GetValidationResult(dto, new ValidationContext(dto));
            var isSuccess = result == ValidationResult.Success;

            isSuccess.Should().BeFalse();
        }

    }
}