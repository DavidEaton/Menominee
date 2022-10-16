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
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 10;
        public static readonly int MinimumWidth = 10;
        public static readonly int MaximumWidth = 999;
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        public static readonly string InvalidTypeLengthMessage = $"Type must be no more than {MaximumTypeLength} characters.";
        public static readonly string InvalidWidthMessage = $"Width must be no more than {MaximumWidth}.";
        public static readonly int AspectRatioLength = 2;
        public static readonly int MinimumDiameter = 1;
        public static readonly int MaximumDiameter = 48;
        public static readonly int MaximumSpeedRatingLength = 3;
        public static readonly string InvalidDiameterMessage = $"Diameter must be between {MinimumLength} and {MaximumDiameter}.";
        public static readonly string InvalidSpeedRatingLengthMessage = $"Speed Rating must be no more than {MaximumSpeedRatingLength} characters.";
        public static readonly string InvalidAspectRatioLengthMessage = $"Aspect Ratio must be no more than {AspectRatioLength} characters.";
        public static readonly int MaximumLoadIndexLength = 3;
        public static readonly string InvalidLoadIndexMessage = $"Load Index must be no more than {MaximumLoadIndexLength} characters.";
        public string Type { get; private set; }

        // Width is ALWAYS a three-digit number in millimeters
        public int Width { get; private set; }

        // Aspect ratio is ALWAYS two digits
        public int AspectRatio { get; private set; }

        // TODO: CONFIRM THIS PROPERTY IS OPTIONAL
        public TireConstructionType ConstructionType { get; private set; }

        // This two-digit number specifies wheel diameter in inches.
        public double Diameter { get; private set; }
        public int LoadIndex { get; private set; }
        public string SpeedRating { get; private set; }

        private InventoryItemTire(int width, int aspectRatio, TireConstructionType constructionType, double diameter,
             double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null, string type = null, int? loadIndex = null, string speedRating = null)
             : base(list, cost, core, retail, techAmount, fractional, lineCode, subLineCode)
        {
            type = (type ?? string.Empty).Trim();
            speedRating = (speedRating ?? string.Empty).Trim();

            if (type?.Length > MaximumTypeLength)
                throw new ArgumentOutOfRangeException(InvalidTypeLengthMessage);

            if (width < MinimumWidth || width  > MaximumWidth)
                throw new ArgumentOutOfRangeException(InvalidWidthMessage);

            if (aspectRatio < MinimumMoneyAmount || aspectRatio.ToString().Length > AspectRatioLength)
                throw new ArgumentOutOfRangeException(InvalidAspectRatioLengthMessage);

            if (!Enum.IsDefined(typeof(TireConstructionType), constructionType))
                throw new ArgumentOutOfRangeException(RequiredMessage);

            if (diameter < MinimumMoneyAmount || diameter > MaximumDiameter)
                throw new ArgumentOutOfRangeException(InvalidDiameterMessage);

            if (loadIndex.HasValue)
                if (loadIndex < MinimumMoneyAmount || loadIndex.ToString().Length > MaximumLoadIndexLength)
                    throw new ArgumentOutOfRangeException(InvalidLoadIndexMessage);

            if (speedRating?.Length > MaximumSpeedRatingLength)
                throw new ArgumentOutOfRangeException(InvalidSpeedRatingLengthMessage);

            Type = type;
            Width = width;
            AspectRatio = aspectRatio;
            ConstructionType = constructionType;
            Diameter = diameter;
            LoadIndex = loadIndex.HasValue
                ? (int)loadIndex
                : 0;
            SpeedRating = speedRating;
        }

        public static Result<InventoryItemTire> Create(int width, int aspectRatio, TireConstructionType constructionType, double diameter, double list, double cost, double core, double retail, TechAmount techAmount, bool fractional, string lineCode = null, string subLineCode = null, string type = null, int? loadIndex = null, string speedRating = null)
        {
            if (list < MinimumMoneyAmount ||
                cost < MinimumMoneyAmount ||
                core < MinimumMoneyAmount ||
                retail < MinimumMoneyAmount ||
                list > MaximumMoneyAmount ||
                cost > MaximumMoneyAmount ||
                core > MaximumMoneyAmount ||
                retail > MaximumMoneyAmount)
                return Result.Failure<InventoryItemTire>(InvalidMoneyAmountMessage);

            lineCode = (lineCode ?? string.Empty).Trim();
            subLineCode = (subLineCode ?? string.Empty).Trim();

            // TechAmount Value Object is validated before we ever get here

            if (lineCode.Length > MaximumLength || subLineCode.Length > MaximumLength)
            return Result.Failure<InventoryItemTire>(InvalidLengthMessage);

            type = (type ?? string.Empty).Trim();
            speedRating = (speedRating ?? string.Empty).Trim();

            if (type.Length > MaximumTypeLength)
                return Result.Failure<InventoryItemTire>(InvalidTypeLengthMessage);

            if (width < MinimumWidth || width > MaximumWidth)
                return Result.Failure<InventoryItemTire>(InvalidWidthMessage);

            if (aspectRatio < MinimumMoneyAmount || aspectRatio.ToString().Length > AspectRatioLength)
                return Result.Failure<InventoryItemTire>(InvalidAspectRatioLengthMessage);

            if (!Enum.IsDefined(typeof(TireConstructionType), constructionType))
                return Result.Failure<InventoryItemTire>(RequiredMessage);

            if (diameter < MinimumMoneyAmount || diameter > MaximumDiameter)
                return Result.Failure<InventoryItemTire>(InvalidDiameterMessage);

            if (loadIndex < MinimumMoneyAmount || loadIndex.ToString().Length > MaximumLoadIndexLength)
                return Result.Failure<InventoryItemTire>(InvalidLoadIndexMessage);

            if (speedRating.Length > MaximumSpeedRatingLength)
                return Result.Failure<InventoryItemTire>(InvalidSpeedRatingLengthMessage);

            return Result.Success(new InventoryItemTire(width, aspectRatio, constructionType, diameter, list, cost, core, retail, techAmount, fractional, lineCode, subLineCode, type, loadIndex, speedRating));
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
            if (width < MinimumWidth || width > MaximumWidth)
                return Result.Failure<int>(InvalidWidthMessage);

            return Result.Success(Width = width);
        }

        public Result<int> SetAspectRatio(int aspectRatio)
        {
            if (aspectRatio < MinimumMoneyAmount || aspectRatio.ToString().Length > AspectRatioLength)
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
            if (diameter < MinimumMoneyAmount || diameter > MaximumDiameter)
                return Result.Failure<double>(InvalidDiameterMessage);

            return Result.Success(Diameter = diameter);
        }

        public Result<int> SetLoadIndex(int loadIndex)
        {
            if (loadIndex < MinimumMoneyAmount || loadIndex.ToString().Length > MaximumLoadIndexLength)
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
