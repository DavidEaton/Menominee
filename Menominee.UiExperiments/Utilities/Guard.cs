using System;

namespace Menominee.UiExperiments.Utilities
{
    public class Guard
    {
        public static void ForLessThanOrEqualToZero(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForPrecedesDate(DateTime value, DateTime dateToPrecede, string parameterName)
        {
            if (value >= dateToPrecede)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }
    }
}
