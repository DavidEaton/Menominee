using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Client.Services.SaleCodes;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryItemForm
    {
        [Inject]
        public IManufacturerDataService manufacturerDataService { get; set; }

        [Inject]
        public ISaleCodeDataService saleCodeDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        [Parameter]
        public InventoryItemToWrite Item { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Item";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        private IReadOnlyList<ManufacturerToReadInList> Manufacturers = null;
        private IReadOnlyList<SaleCodeToReadInList> SaleCodes = null;
        private IReadOnlyList<ProductCodeToReadInList> ProductCodes = null;

        protected override async Task OnParametersSetAsync()
        {
            Manufacturers = (await manufacturerDataService.GetAllManufacturers()).ToList();
            SaleCodes = (await saleCodeDataService.GetAllSaleCodes()).ToList();
            ProductCodes = (await productCodeDataService.GetAllProductCodes()).ToList();

            ManufacturerList = new();
            foreach (var mfr in Manufacturers)
            {
                ManufacturerList.Add(new ManufacturerX
                {
                    Id = mfr.Id,
                    Code = mfr.Code,
                    Prefix = mfr.Prefix,
                    Name = mfr.Name
                });
            }

            SaleCodeList = new();
            foreach (var saleCode in SaleCodes)
            {
                SaleCodeList.Add(new SaleCodeX
                {
                    Id = saleCode.Id,
                    Code = saleCode.Code,
                    Name = saleCode.Name
                });
            }

            ProductCodeList = new();
            foreach (var prodCode in ProductCodes)
            {
                ProductCodeList.Add(new ProductCodeX
                {
                    Id = prodCode.Id,
                    Code = prodCode.Code,
                    Name = prodCode.Name
                });
            }
        }

        private List<ManufacturerX> ManufacturerList = new List<ManufacturerX>();
        private List<SaleCodeX> SaleCodeList = new List<SaleCodeX>();
        private List<ProductCodeX> ProductCodeList = new List<ProductCodeX>();

        public class ManufacturerX
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Prefix { get; set; }
            public string Name { get; set; }
            public string DisplayText
            {
                get
                {
                    return Prefix + " - " + Name;
                }
            }
        }

        public class SaleCodeX
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string DisplayText
            {
                get
                {
                    return Code + " - " + Name;
                }
            }
        }

        public class ProductCodeX
        {
            public long Id { get; set; }
            public string Code { get; set; }
            //public string SaleCode { get; set; }
            public string Name { get; set; }
            public string DisplayText
            {
                get
                {
                    return Code + " - " + Name;
                }
            }
        }
    }
}
