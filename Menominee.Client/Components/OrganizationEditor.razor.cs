using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components
{
    public partial class OrganizationEditor<TItem> : ComponentBase
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public string NameProperty { get; set; }

        [Parameter]
        public string NoteProperty { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }

        public string ItemName
        {
            get
            {
                return Item.GetType().Name;
            }
        }

        public string Name
        {
            get
            {
                return Item.GetType()
                    .GetProperty(NameProperty)
                    .GetValue(Item).ToString();
            }
            set
            {
                Item.GetType()
                    .GetProperty(NameProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

        public string Note
        {
            get
            {
                return Item.GetType()
                    .GetProperty(NoteProperty)
                    .GetValue(Item).ToString();
            }
            set
            {
                Item.GetType()
                    .GetProperty(NoteProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }
    }
}
