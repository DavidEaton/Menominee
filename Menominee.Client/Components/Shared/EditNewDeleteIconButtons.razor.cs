using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Shared
{
    public partial class EditNewDeleteIconButtons
    {
        private EventCallback onEdit;
        private EventCallback onNew;
        private EventCallback onDelete;
        private EventCallback onCancel;

        private bool ShowEdit { get; set; } = false;
        private bool ShowNew { get; set; } = false;
        private bool ShowDelete { get; set; } = false;
        private bool ShowCancel { get; set; } = false;

        [Parameter]
        public bool CanEdit { get; set; } = true;
        [Parameter]
        public bool CanAdd { get; set; } = true;
        [Parameter]
        public bool CanDelete { get; set; } = true;
        [Parameter]
        public bool CanCancel { get; set; } = true;


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
        public EventCallback OnCancel
        {
            get => onCancel;
            set
            {
                onCancel = value;
                ShowCancel = true;
            }
        }

        [Parameter]
        public bool ShowDivider { get; set; } = true;

    }
}
