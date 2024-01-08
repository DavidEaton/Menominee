using FluentValidation;
using Menominee.Client.Shared;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Contactable;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Menominee.Client.Features.Contactables.Emails
{
    public partial class EmailEditor : ComponentBase
    {
        [Inject] IJSRuntime JsInterop { get; set; }

        [Parameter] public EmailToWrite? Email { get; set; }

        [Parameter] public FormMode FormMode { get; set; } = FormMode.Unknown;

        [Parameter] public EventCallback OnSave { get; set; }

        [Parameter] public EventCallback OnCancel { get; set; }
        [Inject] private IValidator<EmailToWrite> Validator { get; set; }
        public string Title { get; set; } = string.Empty;

        private string validationMessage = string.Empty;
        private bool IsValid => string.IsNullOrEmpty(validationMessage);

        public void Save()
        {
            Validate();

            if (!IsValid)
            {
                return;
            }

            OnSave.InvokeAsync();
        }

        private void Validate()
        {
            var validationResult = Validator.Validate(Email);

            if (!validationResult.IsValid)
            {
                validationMessage = $"Please enter a valid email address";
                return;
            }

            validationMessage = string.Empty;
        }


        protected override void OnParametersSet()
        {
            Title = FormTitle.BuildTitle(FormMode, "Email");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JsInterop.InvokeVoidAsync("jsfunction.focusElement", "emailaddress");
            }
        }
    }
}
