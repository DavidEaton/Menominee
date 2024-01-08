using FluentValidation;
using Menominee.Domain.Enums;
using Microsoft.AspNetCore.Components;
using System;

namespace Menominee.Tests.Blazor.Components
{
    public class ComponentTestFixture
    {
        public static void ComponentSetup<TValidator, TRequest>(TestContext context, FormMode formMode, out TRequest request, out string expectedTitle)
        where TValidator : class, IValidator<TRequest>, new()
        where TRequest : class, new()
        {
            context.JSInterop.Mode = JSRuntimeMode.Loose;
            context.JSInterop.SetupVoid("TelerikBlazor.invokeComponentMethod", _ => true);
            context.JSInterop.SetupVoid("TelerikBlazor.initCheckBox", _ => true);
            context.JSInterop.SetupVoid("TelerikBlazor.initComponentLoaderContainer", _ => true);

            context.Services.AddSingleton<IValidator<TRequest>, TValidator>();

            request = new TRequest();
            expectedTitle = $"{formMode} {typeof(TRequest).Name.Replace("ToWrite", "")}";
        }

        public static IRenderedComponent<TestLayout> RenderComponent<TComponent, TRequest>(TestContext context, TRequest request, FormMode formMode)
        where TComponent : ComponentBase, new()
        where TRequest : class
        {
            return context.RenderComponent<TestLayout>(parameters => parameters
                .AddChildContent(CreateRenderFragment<TComponent, TRequest>(request, formMode)));
        }

        private static RenderFragment CreateRenderFragment<TComponent, TRequest>(TRequest request, FormMode formMode)
            where TComponent : ComponentBase, new()
            where TRequest : class
        {
            return builder =>
            {
                builder.OpenComponent<TComponent>(0);

                var componentType = typeof(TComponent);

                if (HasProperty(componentType, "Item", out var itemType) && itemType.IsAssignableFrom(typeof(TRequest)))
                {
                    builder.AddAttribute(1, "Item", request);
                }

                if (HasProperty(componentType, "FormMode", out var formModeType) && formModeType.IsEnum && Enum.IsDefined(formModeType, formMode))
                {
                    builder.AddAttribute(2, "FormMode", formMode);
                }

                builder.CloseComponent();
            };
        }

        private static bool HasProperty(Type type, string propertyName, out Type propertyType)
        {
            var property = type.GetProperty(propertyName);
            propertyType = property?.PropertyType;
            return property != null;
        }
    }
}