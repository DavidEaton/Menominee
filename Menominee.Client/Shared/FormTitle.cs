using Menominee.Domain.Enums;

namespace Menominee.Client.Shared
{
    public class FormTitle
    {
        public static string BuildTitle(FormMode mode, string title)
        {
            return $"{Convert(mode)} {title}";
        }

        private static string Convert(FormMode mode)
        {
            return mode switch
            {
                FormMode.Add => FormMode.Add.ToString(),
                FormMode.Edit => FormMode.Edit.ToString(),
                FormMode.View => FormMode.View.ToString(),
                FormMode.Unknown => FormMode.View.ToString(),
                _ => FormMode.View.ToString(),
            };
        }
    }
}
