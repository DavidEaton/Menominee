using MenomineePlayWASM.Shared.Dtos.Payables.Invoices.Items;
using MenomineePlayWASM.Shared.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.Payables
{
    public partial class VendorInvoiceItemForm
    {
        [Parameter]
        public VendorInvoiceItemToWrite Item { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public FormMode Mode 
        { 
            get => formMode; 
            set
            {
                formMode = value;
                if (formMode == FormMode.Add)
                    Title = "Add";
                else if (formMode == FormMode.Edit)
                    Title = "Edit";
                else
                    Title = "View";
                Title += " Item";
            }
        }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private FormMode formMode;
        public string Title { get; set; }
    }
}
