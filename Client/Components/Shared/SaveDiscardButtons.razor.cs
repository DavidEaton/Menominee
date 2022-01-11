using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.Shared
{
    public partial class SaveDiscardButtons
    {
        [Parameter]
        public EventCallback OnSave { get; set; }
        [Parameter]
        public EventCallback OnDiscard { get; set; }
    }
}
