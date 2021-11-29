using CustomerVehicleManagement.Shared.Models;
using FluentValidation;

namespace CustomerVehicleManagement.Api.Validators
{
    public class CustomerValidator : AbstractValidator<CustomerToWrite>
    {
        public CustomerValidator()
        {


        }
    }
}
