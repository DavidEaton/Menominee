using CustomerVehicleManagement.Api.Validators;
using CustomerVehicleManagement.Shared.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace CustomerVehicleManagement.UnitTests.ValidatorTests
{
    public class EmailsValidatorShould
    {
        private readonly EmailToAddValidator validator;

        public EmailsValidatorShould()
        {
            validator = new EmailToAddValidator();
        }

        //[Fact]
        //public void Have_error_when_Email_has_invalid_Address()
        //{
        //    var email = new EmailToAdd
        //    {
        //        Address = "aa.a",
        //        IsPrimary = true
        //    };

        //    var result = validator.TestValidate(email);
        //    result
        //        .ShouldHaveValidationErrorFor(organization => organization.Emails);
        //    //.WithSeverity(Severity.Error)
        //    //.WithErrorMessage("'Address' is not a valid email address.");
        //}


    }
}
