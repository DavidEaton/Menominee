using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Shared
{
    public partial class EditNewDeleteButtons
    {
        private EventCallback onDone;
        private EventCallback onEdit;
        private EventCallback onNew;
        private EventCallback onDelete;
        private EventCallback onPrint;
        private EventCallback onPrintDirectly;
        private EventCallback onPreview;
        private EventCallback onExport;
        private EventCallback onEmail;

        private bool ShowDone { get; set; } = false;
        private bool ShowEdit { get; set; } = false;
        private bool ShowNew { get; set; } = false;
        private bool ShowDelete { get; set; } = false;
        private bool ShowPrint { get; set; } = false;
        private bool ShowPrintOptions { get; set; } = false;
        private bool ShowPreview { get; set; } = false;
        private bool ShowExport { get; set; } = false;
        private bool ShowEmail { get; set; } = false;

        [Parameter]
        public bool CanEdit { get; set; } = true;
        [Parameter]
        public bool CanAdd { get; set; } = true;
        [Parameter]
        public bool CanDelete { get; set; } = true;
        [Parameter]
        public bool CanPrint { get; set; } = true;

        [Parameter]
        public EventCallback OnDone
        {
            get => onDone;
            set
            {
                onDone = value;
                ShowDone = true;
            }
        }

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
        public EventCallback OnPrint
        {
            get => onPrint;
            set
            {
                onPrint = value;
                ShowPrint = true;
            }
        }

        [Parameter]
        public EventCallback OnPrintDirectly
        {
            get => onPrintDirectly;
            set
            {
                onPrintDirectly = value;
                ShowPrintOptions = true;
            }
        }

        [Parameter]
        public EventCallback OnPreview
        {
            get => onPreview;
            set
            {
                onPreview = value;
                ShowPreview = true;
            }
        }

        [Parameter]
        public EventCallback OnExport
        {
            get => onExport;
            set
            {
                onExport = value;
                ShowExport = true;
            }
        }

        [Parameter]
        public EventCallback OnEmail
        {
            get => onEmail;
            set
            {
                onEmail = value;
                ShowEmail = true;
            }
        }
    }
}
