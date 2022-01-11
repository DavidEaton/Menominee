using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.Shared
{
    public partial class ContentHeader
    {
        [Parameter]
        public string Header { get; set; }
        [Parameter]
        public int HeaderSize { get; set; } = 5;

    }
}
