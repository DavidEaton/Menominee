using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    // Tire Size Explained: What the Numbers Mean
    // https://www.lesschwab.com/article/tire-size-explained-reading-the-sidewall.html
    // https://tiresize.com/tire-size-meaning/

    public class InventoryItemTire : InstallablePart
    {
        public static readonly int MaximumTypeLength = 2;
        public static readonly int MaximumWidthLength = 3;
        public static readonly string InvalidTypeLengthMessage = $"Type must be between {MinimumLength} and {MaximumTypeLength} characters.";
        public static readonly string InvalidWidthLengthMessage = $"Width must be {MaximumWidthLength} characters.";
        public static readonly int AspectRatioLength = 2;
        public static readonly int MaximumDiameter = 30;
        public static readonly int MaximumSpeedRatingLength = 2;
        public static readonly string InvalidDiameterMessage = $"Diameter must be between {MinimumLength} and {MaximumDiameter}.";
        public static readonly string InvalidSpeedRatingLengthMessage = $"Speed Rating must be between {MinimumLength} and {MaximumSpeedRatingLength} characters.";
        public static readonly string InvalidAspectRatioLengthMessage = $"Aspect Ratio must be {AspectRatioLength} characters.";
        public static readonly int MaximumLoadIndexLength = 3;
        public static readonly string InvalidLoadIndexMessage = $"Please limit Load Index to three characters, greater than {MinimumValue}.";
        public string Type { get; private set; }

        // TODO: Shouldn't this be an int?
        // Width is ALWAYS a three-digit number in millimeters
        public int Width { get; private set; }

        // TODO: Shouldn't this be an int? Aspect ratio is ALWAYS two digits
        public int AspectRatio { get; private set; }

        // CONFIRM THIS PROPERTY IS NULLABLE
        public TireConstructionType ConstructionType { get; private set; }

        // TODO: Shouldn't this be an int?
        // This two-digit number specifies wheel diameter in inches.
        public double Diameter { get; private set; }
        public int LoadIndex { get; private set; }
        public string SpeedRating { get; private set; }

        protected InventoryItemTire(string type, int width, int aspectRatio, TireConstructionType constructionType, double diameter, int loadIndex, string speedRating,
             double list, double cost, double core, double retail, TechAmount techAmount, string lineCode, string subLineCode, bool fractional)
             : base(list, cost, core, retail, techAmount, lineCode, subLineCode, fractional)
        {
            type = (type ?? string.Empty).Trim();
            speedRating = (speedRating ?? string.Empty).Trim();

            if (type.Length > MaximumTypeLength)
                throw new ArgumentOutOfRangeException(InvalidTypeLengthMessage);

            if (width < MinimumValue || width.ToString().Length > MaximumWidthLength)
                throw new ArgumentOutOfRangeException(InvalidWidthLengthMessage);

            if (aspectRatio < MinimumValue || aspectRatio.ToString().Length > AspectRatioLength)
                throw new ArgumentOutOfRangeException(InvalidAspectRatioLengthMessage);

            if (!Enum.IsDefined(typeof(TireConstructionType), constructionType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (diameter < MinimumValue || diameter > MaximumDiameter)
                throw new ArgumentOutOfRangeException(InvalidDiameterMessage);

            if (loadIndex < MinimumValue || loadIndex.ToString().Length > MaximumLoadIndexLength)
                throw new ArgumentOutOfRangeException(InvalidLoadIndexMessage);

            if (speedRating.Length < MinimumLength || speedRating.Length > MaximumSpeedRatingLength)
                throw new ArgumentOutOfRangeException(InvalidSpeedRatingLengthMessage);

            Type = type;
            Width = width;
            AspectRatio = aspectRatio;
            ConstructionType = constructionType;
            Diameter = diameter;
            LoadIndex = loadIndex;
            SpeedRating = speedRating;
        }

        public static Result<InventoryItemTire> Create(string type, int width, int aspectRatio, TireConstructionType constructionType, double diameter, int loadIndex, string speedRating, double list, double cost, double core, double retail, TechAmount techAmount, string lineCode, string subLineCode, bool fractional)
        {
            type = (type ?? string.Empty).Trim();
            speedRating = (speedRating ?? string.Empty).Trim();

            if (type.Length > MaximumTypeLength)
                return Result.Failure<InventoryItemTire>(InvalidTypeLengthMessage);

            if (width < MinimumValue || width.ToString().Length > MaximumWidthLength)
                return Result.Failure<InventoryItemTire>(InvalidWidthLengthMessage);

            if (aspectRatio < MinimumValue || aspectRatio.ToString().Length > AspectRatioLength)
                return Result.Failure<InventoryItemTire>(InvalidAspectRatioLengthMessage);

            if (!Enum.IsDefined(typeof(TireConstructionType), constructionType))
                return Result.Failure<InventoryItemTire>(RequiredMessage);

            if (diameter < MinimumValue || diameter > MaximumDiameter)
                return Result.Failure<InventoryItemTire>(InvalidDiameterMessage);

            if (loadIndex < MinimumValue || loadIndex.ToString().Length > MaximumLoadIndexLength)
                return Result.Failure<InventoryItemTire>(InvalidLoadIndexMessage);

            if (speedRating.Length < MinimumLength || speedRating.Length > MaximumSpeedRatingLength)
                return Result.Failure<InventoryItemTire>(InvalidSpeedRatingLengthMessage);

            return Result.Success(new InventoryItemTire(type, width, aspectRatio, constructionType, diameter, loadIndex, speedRating, list, cost, core, retail, techAmount, lineCode, subLineCode, fractional));
        }

        public Result<string> SetType(string type)
        {
            type = (type ?? string.Empty).Trim();

            if (type.Length < MinimumLength || type.Length > MaximumTypeLength)
                return Result.Failure<string>(InvalidTypeLengthMessage);

            return Result.Success(Type = type);
        }

        public Result<int> SetWidth(int width)
        {
            if (width < MinimumValue || width.ToString().Length > MaximumWidthLength)
                return Result.Failure<int>(InvalidWidthLengthMessage);

            return Result.Success(Width = width);
        }

        public Result<int> SetAspectRatio(int aspectRatio)
        {
            if (aspectRatio < MinimumValue || aspectRatio.ToString().Length > AspectRatioLength)
                return Result.Failure<int>(InvalidAspectRatioLengthMessage);

            return Result.Success(AspectRatio = aspectRatio);
        }

        public Result<TireConstructionType> SetConstructionType(TireConstructionType constructionType)
        {
            if (!Enum.IsDefined(typeof(TireConstructionType), constructionType))
                return Result.Failure<TireConstructionType>(RequiredMessage);

            return Result.Success(ConstructionType = constructionType);
        }

        public Result<double> SetDiameter(double diameter)
        {
            if (diameter < MinimumValue || diameter > MaximumDiameter)
                return Result.Failure<double>(InvalidDiameterMessage);

            return Result.Success(Diameter = diameter);
        }

        public Result<int> SetLoadIndex(int loadIndex)
        {
            if (loadIndex < MinimumValue || loadIndex.ToString().Length > MaximumLoadIndexLength)
                return Result.Failure<int>(InvalidLoadIndexMessage);

            return Result.Success(LoadIndex = loadIndex);
        }

        public Result<string> SetSpeedRating(string speedRating)
        {
            speedRating = (speedRating ?? string.Empty).Trim();

            if (speedRating.Length < MinimumLength || speedRating.Length > MaximumSpeedRatingLength)
                return Result.Failure<string>(InvalidSpeedRatingLengthMessage);

            return Result.Success(SpeedRating = speedRating);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemTire() { }

        #endregion
    }
}
