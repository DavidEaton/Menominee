using Menominee.Shared.Models.RepairOrders.SerialNumbers;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumberEditor : ComponentBase
    {
        [Parameter]
        public SerialNumberListItem SerialNumber { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public FormMode Mode
        {
            get => formMode;
            set
            {
                formMode = value;
                Title = FormTitle.BuildTitle(formMode, "Serial Number");
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        protected override void OnInitialized()
        {
            Mode = FormMode.Edit;
        }

        private FormMode formMode;
        private string Title { get; set; }
    }
}
