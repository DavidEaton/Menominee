using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Shared
{
    public class FormTitle
    {
        public static string BuildTitle(FormMode mode, string name)
        {
            string title;
            if (mode == FormMode.Add)
                title = "Add";
            else if (mode == FormMode.Edit)
                title = "Edit";
            else
                title = "View";
            title += (" " + name);
            return title;
        }
    }
}
