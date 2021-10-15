using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Api.Validators
{
    public class PersonToAddValidator : AbstractValidator<PersonToAdd>
    {
        public PersonToAddValidator()
        {

        }
    }
}
