using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components
{
    public partial class PersonNameEditor<TItem> : ComponentBase
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public string FirstNameProperty { get; set; }

        [Parameter]
        public string MiddleNameProperty { get; set; }

        [Parameter]
        public string LastNameProperty { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }

        public string ItemName
        {
            get
            {
                return Item.GetType().Name;
            }
        }

        public string FirstName
        {
            get
            {
                if (Item == null)
                    return string.Empty;

                return Item.GetType()
                    .GetProperty(FirstNameProperty)
                    .GetValue(Item).ToString();
            }
            set
            {
                Item.GetType()
                    .GetProperty(FirstNameProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

        public string MiddleName
        {
            get
            {
                if (Item == null)
                    return string.Empty;

                return Item.GetType()
                    .GetProperty(MiddleNameProperty)
                    .GetValue(Item).ToString();
            }
            set
            {
                Item.GetType()
                    .GetProperty(MiddleNameProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

        public string LastName
        {
            get
            {
                if (Item == null)
                    return string.Empty;

                return Item.GetType()
                    .GetProperty(LastNameProperty)
                    .GetValue(Item).ToString();
            }
            set
            {
                Item.GetType()
                    .GetProperty(LastNameProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }
    }

}
