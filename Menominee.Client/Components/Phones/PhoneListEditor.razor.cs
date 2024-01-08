using Menominee.Domain.Enums;
using Menominee.Shared.Models.Contactable;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Phones
{
    public partial class PhoneListEditor : ComponentBase
    {
        [Parameter]
        public IList<PhoneToWrite>? Phones { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }
        private FormMode PhoneFormMode { get; set; } = FormMode.View;

        [Parameter]
        public PhoneType DefaultPhoneType { get; set; } = PhoneType.Mobile;

        [CascadingParameter]
        public DialogFactory? Dialogs { get; set; }

        private TelerikGrid<PhoneToWrite>? Grid { get; set; }
        private PhoneToWrite? SelectedPhone { get; set; }
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
            SelectedPhone = args.Item as PhoneToWrite;
        }

        private void OnEdit()
        {
            PhoneFormMode = FormMode.Edit;
        }

        private void OnNew()
        {
            SelectedPhone = new PhoneToWrite { PhoneType = DefaultPhoneType };
            Phones?.Add(SelectedPhone);
            PhoneFormMode = FormMode.Add;
        }

        private async Task OnDelete()
        {
            if (Phones is not null && SelectedPhone is not null && Phones.Contains(SelectedPhone))
            {
                var confirm = await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedPhone}?", "Remove Phone");
                if (confirm)
                {
                    Phones.Remove(SelectedPhone);
                    SelectedPhone = null;
                    Grid?.Rebind();
                    PhoneFormMode = FormMode.View;
                }
            }
        }

        private void OnSaveEdit()
        {
            SelectedPhone = null;
            Grid?.Rebind();
            PhoneFormMode = FormMode.View;
        }

        private void OnCancelEdit()
        {
            PhoneFormMode = FormMode.View;
        }
    }
}
