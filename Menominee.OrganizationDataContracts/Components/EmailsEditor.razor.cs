using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class EmailsEditor : ComponentBase
    {
        [Parameter]
        public FormMode FormMode { get; set; }

        [Parameter]
        public IList<EmailToAdd> EmailsToAdd { get; set; }
        public EmailToAdd EmailToAdd { get; set; }

        [Parameter]
        public IList<EmailToEdit> EmailsToEdit { get; set; }
        public EmailToEdit EmailToEdit { get; set; }
        private bool DialogVisible => (EmailToEdit != null && (Adding || Editing)) || (EmailToAdd != null && (Adding || Editing));
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;
        private TelerikTextBox EmailAddressControl { get; set; }

        private async Task AddAsync(string type)
        {
            if (type == "EmailToAdd")
                EmailToAdd = new();

            if (type == "EmailToEdit")
                EmailToEdit = new();

            if (EmailAddressControl != null)
                await EmailAddressControl.FocusAsync();

            Adding = true;
        }

        private void Edit(EmailToEdit item)
        {
            EmailToEdit = item;
            Editing = true;
        }

        private void Save(string type)
        {
            if (type == "EmailToAdd")
                EmailsToAdd.Add(EmailToAdd);

            if (type == "EmailToEdit" && Adding)
                EmailsToEdit.Add(EmailToEdit);

            Adding = false;
            Editing = false;
        }
        private void CancelEditEmail()
        {
            Adding = false;
            Editing = false;
        }

        private void CancelAddEmail()
        {
            if (EmailToAdd != null)
                EmailToAdd = null;

            Adding = false;
            Editing = false;
        }
    }
}
