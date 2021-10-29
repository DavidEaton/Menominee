using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

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
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;
        private void Add(string type)
        {
            if (type == "EmailToAdd")
                EmailToAdd = new();

            if (type == "EmailToEdit")
                EmailToEdit = new();

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

        private void Cancel()
        {
            EmailToAdd = null;
            EmailToEdit = null;
            Adding = false;
            Editing = false;
        }
    }
}
