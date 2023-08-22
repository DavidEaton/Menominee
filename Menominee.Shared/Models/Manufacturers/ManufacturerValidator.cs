using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Manufacturers
{
    // TODO: we need to pass in Lists of existing prefix and ids but the circular dependency issue needs to be taken care of in order to access repo or db.
    public class ManufacturerValidator : AbstractValidator<ManufacturerToWrite>
    {
        public ManufacturerValidator()
        {
            RuleFor(manufacturer => manufacturer.Prefix)
                .NotEmpty()
                .When(manufacturer => manufacturer.Prefix is not null);
            RuleFor(manufacturer => manufacturer)
                .MustBeEntity(manufacturer =>
                Manufacturer.Create(
                    manufacturer.Id,
                    manufacturer.Name,
                    manufacturer.Prefix,
                    new List<string>(),
                    new List<long>())
                );
        }
    }
}
