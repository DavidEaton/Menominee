using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Inventory
{
    public class Manufacturer : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static string InvalidLengthMessage(int minLength, int maxLength) => $"Value must be between {minLength} and {maxLength} characters.";
        public static readonly string UniqueValueMessage = $"Value is already in use and must be unique.";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 255;
        public static readonly int PrefixMaximumLength = 4;
        public static readonly long MinimumIdValue = 1;
        public override long Id
        {
            get => base.Id;
            protected set => base.Id = value;
        }
        public string Name { get; private set; }
        public string Prefix { get; private set; }

        private Manufacturer(long id, string name, string prefix)
        {
            Id = id;
            Name = name;
            Prefix = prefix;
        }

        public static Result<Manufacturer> Create(long id, string name, string prefix, List<string> existingPrefixes, List<long> existingIds)
        {
            name = (name ?? string.Empty).Trim();
            prefix = !string.IsNullOrWhiteSpace(prefix) ? prefix.Trim().ToUpper() : null;

            if (existingIds.Any(value => value == id))
                return Result.Failure<Manufacturer>(UniqueValueMessage);
            if (name.Length > MaximumLength || name.Length < MinimumLength)
                return Result.Failure<Manufacturer>(InvalidLengthMessage(MinimumLength, MaximumLength));
            if (prefix != null && (prefix.Length > PrefixMaximumLength || prefix.Length < MinimumLength))
                return Result.Failure<Manufacturer>(InvalidLengthMessage(MinimumLength, PrefixMaximumLength));
            if (prefix != null && existingPrefixes.Contains(prefix))
                return Result.Failure<Manufacturer>(UniqueValueMessage);

            return Result.Success(new Manufacturer(id, name, prefix));
        }

        public Result<string> SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<string>(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage(MinimumLength, MaximumLength));

            return Result.Success(Name = name);
        }

        public Result<string> SetPrefix(string prefix, List<string> existingPrefixes)
        {
            prefix = !string.IsNullOrWhiteSpace(prefix) ? prefix.Trim().ToUpper() : null;

            if (prefix != null && (prefix.Length > PrefixMaximumLength || prefix.Length < MinimumLength))
                return Result.Failure<string>(InvalidLengthMessage(MinimumLength, PrefixMaximumLength));

            if (prefix != null && existingPrefixes.Contains(prefix))
                return Result.Failure<string>(UniqueValueMessage);

            return Result.Success(Prefix = prefix);
        }


        #region ORM

        // EF requires a parameterless constructor
        protected Manufacturer() { }

        #endregion

    }
}
