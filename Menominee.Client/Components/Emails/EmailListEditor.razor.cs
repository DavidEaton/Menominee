using Menominee.Shared.Models.Contactable;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using Telerik.Blazor;

namespace Menominee.Client.Components.Emails
{
    public partial class EmailListEditor : ComponentBase
    {
        [Parameter]
        public IList<EmailToWrite> Emails { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        private IEnumerable<EmailToWrite> SelectedEmails { get; set; } = Enumerable.Empty<EmailToWrite>();
        private EmailToWrite SelectedEmail { get; set; }
        private EmailToWrite EmailToModify { get; set; } = null;

        private TelerikGrid<EmailToWrite> Grid { get; set; }

        private int SelectedEmailIndex
        {
            get => selectedEmailIndex;
            set
            {
                selectedEmailIndex = value;
                CanEditEmail = selectedEmailIndex >= 0;
                CanDeleteEmail = CanEdit && selectedEmailIndex >= 0;
            }
        }

        private bool CanEdit { get; set; } = false;

        private long EmailIdToSelect { get; set; } = 0;
        private int selectedEmailIndex = -1;
        private bool editDialogVisible = false;

        private FormMode EmailFormMode { get; set; } = FormMode.Unknown;
        private bool CanEditEmail { get; set; } = false;
        private bool CanDeleteEmail { get; set; } = false;

        private bool EditDialogVisible
        {
            get => editDialogVisible;
            set
            {
                if (value == true)
                {
                    if (EmailFormMode == FormMode.Add)
                    {
                        EmailToModify = new();
                        if (Emails.Count == 0)
                            EmailToModify.IsPrimary = true;
                    }

                    if (EmailFormMode == FormMode.Edit || EmailFormMode == FormMode.View)
                    {
                        EmailToModify = new();
                        CopyEmail(SelectedEmail, EmailToModify);
                    }
                }
                else
                {
                    if (EmailToModify is not null)
                        EmailToModify = null;
                    EmailFormMode = FormMode.Unknown;
                }

                editDialogVisible = value;
            }
        }

        protected override void OnParametersSet()
        {
            CanEdit = FormMode == FormMode.Add || FormMode == FormMode.Edit;

            if (Emails?.Count > 0)
            {
                if (EmailIdToSelect == 0)
                    SelectedEmail = Emails.FirstOrDefault();

                SelectedEmailIndex = Emails.IndexOf(SelectedEmail);
                SelectedEmails = new List<EmailToWrite> { SelectedEmail };
            }
        }

        private void OnEdit()
        {
            if (CanEditEmail)
            {
                EmailFormMode = CanEdit ? FormMode.Edit : FormMode.View;
                EditDialogVisible = true;
            }
        }

        private void OnNew()
        {
            EmailFormMode = FormMode.Add;
            EditDialogVisible = true;
        }

        private async Task OnDelete()
        {
            if (SelectedEmail is not null
            && await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedEmail.Address}?", "Remove Email"))
            {
                Emails.Remove(SelectedEmail);
                SelectedEmail = Emails.FirstOrDefault();
                SelectedEmails = new List<EmailToWrite> { SelectedEmail };
                SelectedEmailIndex = Emails.IndexOf(SelectedEmail);
                Grid.Rebind();
            }
        }

        private void OnSaveEdit()
        {
            if (EmailFormMode != FormMode.Add && EmailFormMode != FormMode.Edit)
                return;

            if (EmailFormMode == FormMode.Add)
            {
                Emails.Add(EmailToModify);
                SelectedEmailIndex = Emails.IndexOf(EmailToModify);
                SelectedEmail = Emails[SelectedEmailIndex];
                SelectedEmails = new List<EmailToWrite> { SelectedEmail };
                Grid.Rebind();
            }
            else if (EmailFormMode == FormMode.Edit)
            {
                CopyEmail(EmailToModify, Emails[SelectedEmailIndex]);
            }
            EditDialogVisible = false;
            StateHasChanged();
        }

        private void OnCancelEdit()
        {
            EmailFormMode = FormMode.Unknown;
            EditDialogVisible = false;
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedEmail = args.Item as EmailToWrite;
            SelectedEmailIndex = Emails.IndexOf(SelectedEmail);
            SelectedEmails = new List<EmailToWrite> { SelectedEmail };
        }

        private static void CopyEmail(EmailToWrite src, EmailToWrite dst)
        {
            dst.Address = src.Address;
            dst.IsPrimary = src.IsPrimary;
        }
    }
}
