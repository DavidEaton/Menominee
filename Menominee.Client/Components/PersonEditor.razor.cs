using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components
{
    public partial class PersonEditor<TItem> : ComponentBase
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public bool EnableEditor { get; set; }

        [Parameter]
        public string GenderProperty { get; set; }

        [Parameter]
        public string BirthdayProperty { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }

        public string ItemName
        {
            get
            {
                return Item.GetType().Name;
            }
        }

        public Gender Gender
        {
            get
            {
                if (Item == null)
                    return Gender.Male;

                return (Gender)Convert.ToInt32(Item.GetType()
                    .GetProperty(GenderProperty)
                    .GetValue(Item));
            }
            set
            {
                Item.GetType()
                    .GetProperty(GenderProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

        public DateTime? Birthday
        {
            get
            {
                if (Item == null)
                    return null;

                return Convert.ToDateTime(Item.GetType()
                    .GetProperty(BirthdayProperty)
                    .GetValue(Item));
            }
            set
            {
                Item.GetType()
                    .GetProperty(BirthdayProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

        protected override void OnInitialized()
        {
            foreach (Gender item in Enum.GetValues(typeof(Gender)))
            {
                GenderEnumData.Add(new GenderEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        List<GenderEnumModel> GenderEnumData { get; set; } = new List<GenderEnumModel>();

    }
    public class GenderEnumModel
    {
        public Gender Value { get; set; }
        public string DisplayText { get; set; }
    }

}
