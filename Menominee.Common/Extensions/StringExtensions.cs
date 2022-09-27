using System;

namespace Menominee.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
            else
                return value.Substring(0, Math.Min(value.Length, length));
        }
    }
}