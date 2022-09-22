using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Menominee.Common.Enums
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue?.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName()
                ??
                enumValue.ToString();
        }

        public static T GetValueFromName<T>(this string name) where T : Enum
        {
            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                    if (attribute.Name == name)
                        return (T)field.GetValue(null);

                if (field.Name == name)
                    return (T)field.GetValue(null);
            }

            throw new ArgumentOutOfRangeException(nameof(name));
        }
    }
}