using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.Shared
{
    public partial class EditNewDeleteIconButtons
    {
        private EventCallback onEdit;
        private EventCallback onNew;
        private EventCallback onDelete;

        private bool ShowEdit { get; set; } = false;
        private bool ShowNew { get; set; } = false;
        private bool ShowDelete { get; set; } = false;

        [Parameter]
        public bool CanEdit { get; set; } = true;
        [Parameter]
        public bool CanAdd { get; set; } = true;
        [Parameter]
        public bool CanDelete { get; set; } = true;


        [Parameter]
        public EventCallback OnEdit
        {
            get => onEdit;
            set
            {
                onEdit = value;
                ShowEdit = true;
            }
        }

        [Parameter]
        public EventCallback OnNew
        {
            get => onNew;
            set
            {
                onNew = value;
                ShowNew = true;
            }
        }

        [Parameter]
        public EventCallback OnDelete
        {
            get => onDelete;
            set
            {
                onDelete = value;
                ShowDelete = true;
            }
        }

        [Parameter]
        public bool ShowDivider { get; set; } = true;

    }
}
