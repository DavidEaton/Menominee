using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Shared
{
    public enum CardBackgroundColor
    {
        white,
        extraLight,
        light,
        medium,
        dark,
        extraDark
    };

    public partial class Card : ComponentBase
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment Content { get; set; }

        [Parameter]
        public CardBackgroundColor BackgroundColor { get; set; } = CardBackgroundColor.extraLight;

        [Parameter]
        public bool Collapsible { get; set; } = false;

        [Parameter]
        public bool Collapsed { get; set; } = false;

        private void ToggleExpanded()
        {
            Collapsed = !Collapsed;
        }

        private string CardColor()
        {
            switch (BackgroundColor)
            {
                case CardBackgroundColor.white:
                    return "m-card-bg-white";
                case CardBackgroundColor.extraLight:
                    return "m-card-bg-extra-light";
                case CardBackgroundColor.light:
                    return "m-card-bg-light";
                case CardBackgroundColor.medium:
                    return "m-card-bg-medium";
                case CardBackgroundColor.dark:
                    return "m-card-bg-dark";
                case CardBackgroundColor.extraDark:
                    return "m-card-bg-extra-dark";
            }

            return "";
        }
    }
}
