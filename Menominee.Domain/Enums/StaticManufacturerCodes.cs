namespace Menominee.Domain.Enums
{
    // TODO: Where should this class exist - it's not an enum
    // Enumeration pattern?
    // ChatGPT: 
    // "The structure presented in the StaticManufacturerCodes class is known
    // as a static class with constant fields. It resembles the idea of an
    // enumeration, where each field represents a unique value, but it's
    // explicitly defined using constants instead of an enum type. This
    // pattern is often used to represent a fixed set of related constants
    // that are known at compile time and will not change." Approved by DE
    public static class StaticManufacturerCodes
    {
        public const long Custom = 0;
        public const long Miscellaneous = 1;
        public const long CustomStocked = 2;
        public const long Package = 3;
    }
}
