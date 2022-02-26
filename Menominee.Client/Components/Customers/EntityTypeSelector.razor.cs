using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components.Customers
{
    public partial class EntityTypeSelector : ComponentBase
    {
        [Parameter]
        public EntityType EntityType { get; set; }

        List<EntityTypeEnumModel> EntityTypeEnumData { get; set; } = new List<EntityTypeEnumModel>();

        [Parameter]
        public EventCallback<string> OnEntityTypeChanged { get; set; }

        public void EntityTypeChanged(object arg)
        {
            //string entityType = (string)arg;

            try
            {
                string entityType = Convert.ToString(arg);

                if (entityType == EntityType.Organization.ToString())
                    OnEntityTypeChanged.InvokeAsync(EntityType.Organization.ToString());

                if (entityType == EntityType.Person.ToString())
                    OnEntityTypeChanged.InvokeAsync(EntityType.Person.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void OnInitialized()
        {
            foreach (EntityType item in Enum.GetValues(typeof(EntityType)))
            {
                EntityTypeEnumData.Add(new EntityTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }
        private class EntityTypeEnumModel
        {
            public EntityType Value { get; set; }
            public string DisplayText { get; set; }
        }
    }
}
