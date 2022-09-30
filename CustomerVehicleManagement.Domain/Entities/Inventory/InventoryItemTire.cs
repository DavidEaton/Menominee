using CSharpFunctionalExtensions;
using System;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    // Tire Size Explained: What the Numbers Mean
    // https://www.lesschwab.com/article/tire-size-explained-reading-the-sidewall.html
    // https://tiresize.com/tire-size-meaning/

    public class InventoryItemTire : InstallablePart
    {
        public static readonly int MinimumLength = 0;
        public static readonly int MaximumTypeLength = 2;
        public static readonly string InvalidTypeLengthMessage = $"Type must be between {MinimumLength} and {MaximumTypeLength}.";
        public static readonly int MaximumSpeedRatingLength = 1;
        public static readonly int MaximumPrecisionWidth = 2;
        public static readonly string InvalidSpeedRatingLengthMessage = $"Type must be between {MinimumLength} and {MaximumSpeedRatingLength}.";
        public static readonly int MinimumValue = 0;
        public static readonly int MaximumValue = 99999;
        public static readonly int MaximumLoadIndex = 999;
        public static readonly string InvalidValueMessage = $"Value must be between {MinimumValue} and {MaximumValue}.";
        public static readonly string InvalidLoadIndexMessage = $"Please limit Load Index to three characters, greater than {MinimumValue}.";
        public string Type { get; private set; }
        public double Width { get; private set; }
        public double AspectRatio { get; private set; }
        public double Diameter { get; private set; }
        public int LoadIndex { get; private set; }
        public string SpeedRating { get; private set; }

        private InventoryItemTire(string type, double width, double aspectRatio, double diameter, int loadIndex,
            string speedRating)
        {
            type = (type ?? string.Empty).Trim();
            speedRating = (speedRating ?? string.Empty).Trim();

            if (type.Length < MinimumLength || type.Length > MaximumTypeLength)
                throw new ArgumentOutOfRangeException(InvalidTypeLengthMessage);

            if (speedRating.Length < MinimumLength || speedRating.Length > MaximumSpeedRatingLength)
                throw new ArgumentOutOfRangeException(InvalidSpeedRatingLengthMessage); 

            if (width < MinimumValue || width > MaximumValue ||
                aspectRatio < MinimumValue || aspectRatio > MaximumValue ||
                diameter < MinimumValue || diameter > MaximumValue)
                throw new ArgumentOutOfRangeException(InvalidValueMessage);

            if (loadIndex > MaximumLoadIndex || loadIndex < MinimumValue)
                throw new ArgumentOutOfRangeException(InvalidLoadIndexMessage);

            Type = type;
            Width = width;
            AspectRatio = aspectRatio;
            Diameter = diameter;
            LoadIndex = loadIndex;
            SpeedRating = speedRating;
        }

        public static Result<InventoryItemTire> Create(string type, double width, double aspectRatio, double diameter, int loadIndex, string speedRating)
        {
            type = (type ?? string.Empty).Trim();
            speedRating = (speedRating ?? string.Empty).Trim();

            if (type.Length < MinimumLength || type.Length > MaximumTypeLength)
                return Result.Failure<InventoryItemTire>(InvalidTypeLengthMessage);

            if (speedRating.Length < MinimumLength || speedRating.Length > MaximumSpeedRatingLength)
                return Result.Failure<InventoryItemTire>(InvalidSpeedRatingLengthMessage);

            if (width < MinimumValue || width > MaximumValue || aspectRatio < MinimumValue || aspectRatio > MaximumValue || diameter < MinimumValue || diameter > MaximumValue)
                return Result.Failure<InventoryItemTire>(InvalidValueMessage);

            if (loadIndex > MaximumLoadIndex || loadIndex < MinimumValue)
                return Result.Failure<InventoryItemTire>(InvalidLoadIndexMessage);

            return Result.Success(new InventoryItemTire(type, width, aspectRatio, diameter, loadIndex, speedRating));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemTire() { }

        #endregion
    }
}
