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
        private bool AddingEmail { get; set; } = false;
        private void AddEmail(string type)
        {
            if (type == "EmailToAdd")
                EmailToAdd = new();

            if (type == "EmailToEdit")
                EmailToEdit = new();

            AddingEmail = true;
        }

        private void SaveEmail(string type)
        {
            if (type == "EmailToAdd")
                EmailsToAdd.Add(EmailToAdd);

            if (type == "EmailToEdit")
                EmailsToEdit.Add(EmailToEdit);

            AddingEmail = false;
        }

        private void Cancel()
        {
            EmailToAdd = null;
            EmailToEdit = null;
            AddingEmail = false;
        }
    }
}
