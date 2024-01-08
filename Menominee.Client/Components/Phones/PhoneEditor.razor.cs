using FluentValidation;
using Menominee.Client.Shared;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Contactable;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Phones
{
    public partial class PhoneEditor
    {
        [Parameter] public PhoneToWrite? Phone { get; set; }

        [Parameter] public FormMode FormMode { get; set; } = FormMode.Unknown;

        [Parameter] public EventCallback OnSave { get; set; }

        [Parameter] public EventCallback OnCancel { get; set; }

        [Inject] private IValidator<PhoneToWrite> Validator { get; set; }

        public string Title { get; set; } = string.Empty;
        private List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();
        private bool parametersSet = false;
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

        protected override void OnInitialized()
        {
            foreach (PhoneType item in Enum.GetValues(typeof(PhoneType)))
            {
                if (item != PhoneType.Unknown)
                {
                    PhoneTypeEnumData.Add(new PhoneTypeEnumModel { DisplayText = item.ToString(), Value = item });
                }
            }
        }

        private void Validate()
        {
            var validationResult = Validator.Validate(Phone);

            if (!validationResult.IsValid)
            {
                validationMessage = $"Please enter a valid phone number";
                return;
            }

            validationMessage = string.Empty;
        }

        protected override void OnParametersSet()
        {
            Title = FormTitle.BuildTitle(FormMode, "Phone");

            if (!parametersSet)
            {
                return;
            }

            parametersSet = true;
        }

        internal class PhoneTypeEnumModel
        {
            public PhoneType Value { get; set; }
            public string DisplayText { get; set; } = string.Empty;
        }
    }
}
