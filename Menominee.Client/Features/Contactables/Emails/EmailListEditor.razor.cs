using Menominee.Domain.Enums;
using Menominee.Shared.Models.Contactable;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Features.Contactables.Emails
{
    public partial class EmailListEditor : ComponentBase
    {
        [Parameter]
        public IList<EmailToWrite>? Emails { get; set; } = null;

        [Parameter]
        public FormMode FormMode { get; set; }
        private FormMode EmailFormMode { get; set; } = FormMode.View;

        [CascadingParameter]
        public DialogFactory? Dialogs { get; set; }
        private TelerikGrid<EmailToWrite>? Grid { get; set; }

        private EmailToWrite? SelectedEmail { get; set; }
        private bool parametersSet = false;
        private bool CanAddEditDelete => FormMode is FormMode.Add or FormMode.Edit;

        protected override void OnParametersSet()
        {
            if (parametersSet)
            {
                return;
            }

            parametersSet = true;
        }
        private void SelectRow(GridRowClickEventArgs args)
        {
            SelectedEmail = args.Item as EmailToWrite;
        }

        private void OnEdit()
        {
            EmailFormMode = FormMode.Edit;
        }

        private void OnNew()
        {
            SelectedEmail = new();
            Emails?.Add(SelectedEmail);
            EmailFormMode = FormMode.Add;
        }

        private async Task OnDelete()
        {
            if (Emails is not null && SelectedEmail is not null && Emails.Contains(SelectedEmail))
            {
                var confirm = await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedEmail}?", "Remove Phone");
                if (confirm)
                {
                    Emails.Remove(SelectedEmail);
                    SelectedEmail = null;
                    Grid?.Rebind();
                    EmailFormMode = FormMode.View;
                }
            }
        }

        private void OnSaveEdit()
        {
            SelectedEmail = null;
            Grid?.Rebind();
            EmailFormMode = FormMode.View;
        }

        private void OnCancelEdit()
        {
            EmailFormMode = FormMode.View;
        }
    }
}
