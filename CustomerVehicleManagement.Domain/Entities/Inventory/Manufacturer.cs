using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class Manufacturer : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public string Name { get; private set; }
        public string Prefix { get; private set; }
        public string Code { get; private set; }

        private Manufacturer(string name, string prefix, string code)
        {
            Name = name;
            Prefix = prefix;
            Code = code;
        }

        public static Result<Manufacturer> Create(string name, string prefix, string code)
        {
            name = (name ?? string.Empty).Trim();
            prefix = (prefix ?? string.Empty).Trim();
            code = (code ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength ||
                prefix.Length > MaximumLength || prefix.Length < MinimumLength ||
                code.Length > MaximumLength || code.Length < MinimumLength)
                return Result.Failure<Manufacturer>(InvalidLengthMessage);

            return Result.Success(new Manufacturer(name, prefix, code));
        }

        public Result<string> SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<string>(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Name = name);
        }

        public Result<string> SetPrefix(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return Result.Failure<string>(RequiredMessage);

            prefix = (prefix ?? string.Empty).Trim();

            if (prefix.Length > MaximumLength || prefix.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Prefix = prefix);
        }

        public Result<string> SetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Result.Failure<string>(RequiredMessage);

            code = (code ?? string.Empty).Trim();

            if (code.Length > MaximumLength || code.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Code = code);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Manufacturer() { }

        #endregion

    }
}
