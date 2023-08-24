using FluentValidation;
using Menominee.Domain.Entities.Inventory;
using System.Collections.Generic;
namespace Menominee.Shared.Models.ProductCodes
{
    //TODO: replace new List<>() instances once circular dependency is resolved
    public class ProductCodeValidator : AbstractValidator<ProductCodeToWrite>
    {
        public ProductCodeValidator()
        {
            RuleFor(productCode => productCode)
                .MustBeEntity(productCode =>
                ProductCode.Create(
                    Manufacturer.Create(productCode.Manufacturer.Id, productCode.Manufacturer.Name, productCode.Manufacturer.Prefix, new List<string>(), new List<long>()).Value,
                    productCode.Code,
                    productCode.Name,
                    new List<string>().AsReadOnly()
                    )
                );
        }
    }
}
