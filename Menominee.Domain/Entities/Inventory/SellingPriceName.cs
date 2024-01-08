using CSharpFunctionalExtensions;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities.Inventory;

public class SellingPriceName : Entity
{
    public static readonly int MinimumNameLength = 1;
    public static readonly int MaximumNameLength = 20;

    public static readonly string RequiredMessage = $"Name is required";
    public static readonly string InvalidLengthMessage = $"Name must be between {MinimumNameLength} and {MaximumNameLength} characters in length";

    public string Name { get; private set; }

    private SellingPriceName(string name)
    {
        Name = name;
    }

    public static Result<SellingPriceName> Create(string name)
    {
        var nameResult = ValidateName(name);
        if (nameResult.IsFailure)
        {
            return Result.Failure<SellingPriceName>(nameResult.Error);
        }

        return Result.Success(new SellingPriceName(nameResult.Value));
    }

    private static Result<string> ValidateName(string name)
    {
        name = (name ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<string>(RequiredMessage);
        }

        return name.Length < MinimumNameLength || name.Length > MaximumNameLength
            ? Result.Failure<string>(InvalidLengthMessage)
            : Result.Success(name);
    }

    public Result<string> SetName(string name)
    {
        var result = ValidateName(name);

        return result.IsFailure
            ? Result.Failure<string>(result.Error)
            : Result.Success(Name = result.Value);
    }
}
